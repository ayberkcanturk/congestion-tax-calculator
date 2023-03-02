using Microsoft.Extensions.DependencyInjection;
using Volvo.CongestionTax.Domain.Core;
using Volvo.CongestionTax.Domain.Services;

namespace Volvo.CongestionTax.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IDomainEventService, DomainEventService>();
            services.AddTransient<ICongestionTaxService, CongestionTaxService>();

            return services;
        }
    }
}