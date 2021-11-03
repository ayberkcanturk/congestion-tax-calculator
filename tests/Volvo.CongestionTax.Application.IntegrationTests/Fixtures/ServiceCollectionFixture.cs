using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Volvo.CongestionTax.WebAPI;

namespace Volvo.CongestionTax.Application.IntegrationTests.Fixtures
{
    public class ServiceCollectionFixture : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ServiceProvider _serviceProvider;

        public ServiceCollectionFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var startup = new Startup(configuration);

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "Volvo.CongestionTax.WebAPI"));

            startup.ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }
    }
}