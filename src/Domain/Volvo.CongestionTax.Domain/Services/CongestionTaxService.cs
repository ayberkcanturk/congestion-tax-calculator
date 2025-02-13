﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volvo.CongestionTax.Data.Core.Repositories;
using Volvo.CongestionTax.Domain.Core;
using Volvo.CongestionTax.Domain.Entities;
using Volvo.CongestionTax.Domain.EqualityComparers;
using Volvo.CongestionTax.Domain.Events;
using Volvo.CongestionTax.Domain.Exceptions;
using Volvo.CongestionTax.Domain.ValueObjects;

namespace Volvo.CongestionTax.Domain.Services
{
    public class CongestionTaxService : ICongestionTaxService
    {
        private static readonly DateEqualityComparer DateEqualityComparer = new();
        private readonly IRepository<CityCongestionTaxRules> _cityCongestionTaxRulesRepository;
        private readonly IDomainEventService _domainEventService;
        private readonly IRepository<PublicHoliday> _publicHolidaysRepository;

        public CongestionTaxService(IDomainEventService domainEventService,
            IRepository<CityCongestionTaxRules> cityCongestionTaxRulesRepository,
            IRepository<PublicHoliday> publicHolidaysRepository)
        {
            _domainEventService = domainEventService;
            _cityCongestionTaxRulesRepository = cityCongestionTaxRulesRepository;
            _publicHolidaysRepository = publicHolidaysRepository;
        }

        public async Task<decimal> CalculateAsync(string countryCode,
            string city,
            string vehicleType,
            IList<DateTime> passageDates,
            CancellationToken cancellationToken = default)
        {
            var cityCongestionTaxRules =
                await GetCityCongestionTaxRulesByCountryCodeAndCity(countryCode, city, cancellationToken);

            if (cityCongestionTaxRules == null) throw new CongestionTaxRulesNotFoundException(countryCode, city);

            if (IsTaxExemptVehicle(cityCongestionTaxRules, vehicleType))
                return 0;

            var publicHolidaysForCountry = await GetPublicHolidaysByCountryCode(countryCode, cancellationToken);

            var totalAmount = CalculateByTaxRulesPublicHolidaysAndPassageDates(cityCongestionTaxRules,
                publicHolidaysForCountry, passageDates, cancellationToken);

            await _domainEventService.Publish(new CongestionTaxCalculatedEvent
            {
                City = city,
                VehicleType = vehicleType,
                PassagesTimes = passageDates,
                Amount = totalAmount
            });

            return totalAmount;
        }

        private static decimal CalculateByTaxRulesPublicHolidaysAndPassageDates(
            CityCongestionTaxRules cityCongestionTaxRules,
            ICollection<PublicHoliday> publicHolidays, IList<DateTime> passageDates,
            CancellationToken cancellationToken = default)
        {
            decimal totalAmount = 0;

            var distinctPassageDates = passageDates.Distinct(DateEqualityComparer);

            var distinctTollFreeDates = cityCongestionTaxRules
                .TollFreeDates
                .Distinct(DateEqualityComparer)
                .ToList();

            foreach (var distinctPassageDate in distinctPassageDates)
            {
                if (distinctTollFreeDates.Any(d => d.Date == distinctPassageDate.Date)) continue;

                if (IsTaxFreePassageDate(cityCongestionTaxRules, distinctPassageDate, publicHolidays)) continue;

                totalAmount +=
                    GetTotalAmountForADay(cityCongestionTaxRules,
                        passageDates.Where(d => d.Date == distinctPassageDate.Date).ToList(), cancellationToken);
            }

            return totalAmount;
        }

        private async Task<CityCongestionTaxRules> GetCityCongestionTaxRulesByCountryCodeAndCity(string countryCode,
            string city,
            CancellationToken cancellationToken = default)
        {
            return await _cityCongestionTaxRulesRepository.FindOneAsync(c =>
                c.CountryCode == countryCode
                && c.City == city, cancellationToken);
        }

        private async Task<ICollection<PublicHoliday>> GetPublicHolidaysByCountryCode(string countryCode,
            CancellationToken cancellationToken = default)
        {
            return await _publicHolidaysRepository.FindAsync(h => h.IsActive
                                                                  && h.CountryCode == countryCode, cancellationToken);
        }

        private static bool IsTaxExemptVehicle(CityCongestionTaxRules cityCongestionTaxRules, string vehicleType)
        {
            return cityCongestionTaxRules.TaxExemptVehicles.Any(v => v.IsActive && v.Type == vehicleType);
        }

        private static bool IsTaxFreePassageDate(CityCongestionTaxRules cityCongestionTaxRules, DateTime passageDate,
            ICollection<PublicHoliday> publicHolidays)
        {
            return passageDate.DayOfWeek == DayOfWeek.Saturday ||
                   passageDate.DayOfWeek == DayOfWeek.Sunday ||
                   publicHolidays.Any(x => x.Date == passageDate.Date) ||
                   publicHolidays.Any(x =>
                       passageDate >= x.Date.AddHours(-cityCongestionTaxRules.HoursForFreeBeforeEachHolidayStart)
                       && passageDate < x.Date);
        }

        private static decimal GetTotalAmountForADay(CityCongestionTaxRules cityCongestionTaxRules,
            IEnumerable<DateTime> passageDates, CancellationToken cancellationToken = default)
        {
            decimal totalDailyAmount = 0;
            decimal previousTollAmount = 0;
            DateTime? previousPassageDate = null;

            void SetPaidToll(TimeZoneAmount timeZoneAmount, DateTime passageDate)
            {
                totalDailyAmount += timeZoneAmount.Amount;
                previousTollAmount = timeZoneAmount.Amount;
                previousPassageDate = passageDate;
            }

            foreach (var currentPassageDate in passageDates)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var timeZoneAmount = cityCongestionTaxRules.TimeZoneAmounts
                    .First(t => t.TimeZone.IsInTimeZone(currentPassageDate));

                if (previousPassageDate.HasValue)
                {
                    var totalMinutesSinceLastPaidToll = (currentPassageDate - previousPassageDate.Value).TotalMinutes;

                    if (totalMinutesSinceLastPaidToll <= cityCongestionTaxRules.MinutesForFreeAfterAPassage)
                    {
                        if (previousTollAmount < timeZoneAmount.Amount)
                        {
                            totalDailyAmount += timeZoneAmount.Amount - previousTollAmount;
                            previousTollAmount = timeZoneAmount.Amount;
                        }
                    }
                    else
                    {
                        SetPaidToll(timeZoneAmount, currentPassageDate);
                    }
                }
                else
                {
                    SetPaidToll(timeZoneAmount, currentPassageDate);
                }

                if (totalDailyAmount >= cityCongestionTaxRules.MaxDailyTollAmount)
                    return cityCongestionTaxRules.MaxDailyTollAmount;
            }

            return totalDailyAmount;
        }
    }
}