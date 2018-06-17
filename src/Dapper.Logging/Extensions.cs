using System;
using System.Data.Common;
using Dapper.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    public static class Extensions
    {
        /// <summary>
        /// Registers the LogDbConnectionFactory in the IoC container 
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The connection factory delegate</param>
        /// <param name="config">The configuration delegate</param>
        /// <param name="lifetime">The service lifetime</param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnectionFactory(
            this IServiceCollection services, 
            Func<IServiceProvider, DbConnection> factory,
            Action<DbLoggingConfigurationBuilder> config = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var builder = new DbLoggingConfigurationBuilder();
            config?.Invoke(builder);

            object FactoryWrapper(IServiceProvider x) => new LogDbConnectionFactory(
                x.GetService<ILogger<DbConnection>>(), 
                () => factory(x),
                builder.Build());

            services.TryAdd(new ServiceDescriptor(typeof(IDbConnectionFactory), FactoryWrapper, lifetime));
            return services;
        }
    }
}