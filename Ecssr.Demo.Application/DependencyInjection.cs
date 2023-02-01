using Ecssr.Demo.Application.Common.Behaviour;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecssr.Demo.Application
{
    /// <summary>
    /// This class is part of the DI. This has been implemented seperately so that it is part of SOC. Also it becomes to handle all applciation related DIs in one place.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //adding Automapper to DI
            _ = services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //adding Mediatr to DI
            _ = services.AddMediatR(Assembly.GetExecutingAssembly());

            //Adding FLuentValidtion to DI
            _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);
                        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}