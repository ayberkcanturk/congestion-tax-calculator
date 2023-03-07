using System;
using System.Collections.Generic;
using Volvo.CongestionTax.Application.Queries;

namespace Volvo.CongestionTax.Tests.Common
{
    public class InvalidCommands
    {
        public static IEnumerable<object[]> CalculateCongestionTaxCommands =>
            new List<object[]>
            {
                new object[]
                {
                    new CalculateCongestionTaxQuery
                    {
                        CountryCode = "",
                        City = "Gothenburg",
                        VehicleType = "Car",
                        PassagesTimes = new List<DateTime>
                        {
                            DateTime.Today
                        }
                    }
                },
                new object[]
                {
                    new CalculateCongestionTaxQuery
                    {
                        CountryCode = "SE",
                        City = "",
                        VehicleType = "Car",
                        PassagesTimes = new List<DateTime>
                        {
                            DateTime.Today
                        }
                    }
                },
                new object[]
                {
                    new CalculateCongestionTaxQuery
                    {
                        CountryCode = "SE",
                        City = "Gothenburg",
                        VehicleType = "",
                        PassagesTimes = new List<DateTime>
                        {
                            DateTime.Today
                        }
                    }
                },
                new object[]
                {
                    new CalculateCongestionTaxQuery
                    {
                        CountryCode = "SE",
                        City = "Gothenburg",
                        VehicleType = "Car",
                        PassagesTimes = new List<DateTime>()
                    }
                }
            };
    }
}