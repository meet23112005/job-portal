using FluentValidation;
using Job_portal.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Job_portal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            // scans current assembly automatically
            var assembly = typeof(DependencyInjection).Assembly;


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddAutoMapper(cfg => cfg.AddMaps(assembly));


            // registers all Validators
            services.AddValidatorsFromAssembly(assembly);

            // plugs into MediatR pipeline
            // runs BEFORE every handler automatically
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
