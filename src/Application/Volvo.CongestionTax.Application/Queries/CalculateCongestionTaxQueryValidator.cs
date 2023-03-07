using FluentValidation;

namespace Volvo.CongestionTax.Application.Queries
{
    public class CalculateCongestionTaxCommandValidator : AbstractValidator<CalculateCongestionTaxQuery>
    {
        public CalculateCongestionTaxCommandValidator()
        {
            RuleFor(c => c.City).NotNull().NotEmpty();
            RuleFor(c => c.CountryCode).NotNull().NotEmpty();
            RuleFor(c => c.VehicleType).NotNull().NotEmpty();
            RuleFor(c => c.PassagesTimes).NotNull().NotEmpty();
        }
    }
}