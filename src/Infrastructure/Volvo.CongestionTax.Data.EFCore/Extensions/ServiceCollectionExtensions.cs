using Microsoft.Extensions.DependencyInjection;
using Volvo.CongestionTax.Data.Core.Repositories;
using Volvo.CongestionTax.Data.EFCore.Repository;

namespace Volvo.CongestionTax.Data.EFCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataEfCore(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddTransient<CongestionTaxDbContext>();

            return services;
        }
    }
}