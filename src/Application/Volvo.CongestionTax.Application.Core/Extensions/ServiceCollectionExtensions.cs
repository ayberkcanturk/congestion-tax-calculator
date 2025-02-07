﻿using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Volvo.CongestionTax.Application.Core.Behaviour;

namespace Volvo.CongestionTax.Application.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSharedKernel(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

            return services;
        }
    }
}