using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;

namespace Mimir.API
{
    public static class ServiceManager
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RegisterCommandHandlers(services);
            RegisterQueryHandlers(services);
            RegisterAutomapper(services);
        }

        private static void RegisterAutomapper(IServiceCollection services)
        {
            var mapperConfigs = Assembly.GetExecutingAssembly()
              .GetTypes()
              .Where(x => x.IsSubclassOf(typeof(Profile)))
              .ToList();
            var config = new MapperConfiguration(cfg => mapperConfigs.ForEach(a => cfg.AddProfile(a)));
            
            services.AddSingleton(config);
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
