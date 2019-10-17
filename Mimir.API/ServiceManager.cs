using Microsoft.Extensions.DependencyInjection;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;
using System;
using System.Linq;
using System.Reflection;

namespace Mimir.API
{
    public static class ServiceManager
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RegisterCommandHandlers(services);
            RegisterQueryHandlers(services);
        }

        private static void RegisterQueryHandlers(IServiceCollection services)
        {
            services.AddScoped<QueryDispatcher>();
            var queryHandlers = Assembly.GetAssembly(typeof(IQueryHandler<,>))
                 .GetTypes()
                 .Where(x => x.GetInterfaces().Any(x => x.IsGenericType
                                                    && x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handler in queryHandlers)
            {
                var implementedInterfaces = handler.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .Where(x => x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                foreach (var @interface in implementedInterfaces)
                {
                    services.AddScoped(@interface, handler);
                }
            }
        }

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            services.AddScoped<CommandDispatcher>();
            var commandHandlers = Assembly.GetAssembly(typeof(ICommandHandler<>))
                .GetTypes()
                .Where(x => x.GetInterfaces().Any(x => x.IsGenericType 
                                                    && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var handler in commandHandlers)
            {
                var implementedInterfaces = handler.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .Where(x=>x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                foreach (var @interface in implementedInterfaces)
                {
                    services.AddScoped(@interface, handler);
                }
            }
        }
    }
}
