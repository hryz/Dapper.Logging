using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper.Logging.Configuration;
using Dapper.Logging.Hooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    public static class Extensions
    {
        /// <summary>
        /// Registers DbConnectionFactory (with logging) in the IoC container 
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The connection factory delegate</param>
        /// <param name="config">The configuration delegate</param>
        /// <param name="lifetime">The service lifetime</param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnectionFactory(
            this IServiceCollection services, 
            Func<IServiceProvider, DbConnection> factory,
            Func<DbLoggingConfigurationBuilder, DbLoggingConfigurationBuilder> config = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var builder = new DbLoggingConfigurationBuilder();
            builder = config?.Invoke(builder) ?? builder;

            object FactoryWrapper(IServiceProvider x) => 
                new ContextlessLoggingFactory(
                    x.GetService<ILogger<IDbConnectionFactory>>(), 
                    builder.Build(),
                    () => factory(x));

            services.TryAdd(new ServiceDescriptor(typeof(IDbConnectionFactory), FactoryWrapper, lifetime));
            return services;
        }
        
        /// <summary>
        /// Registers DbConnectionFactory (with context logging) in the IoC container 
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The connection factory delegate</param>
        /// <param name="config">The configuration delegate</param>
        /// <param name="lifetime">The service lifetime</param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnectionFactoryWithCtx<T>(
            this IServiceCollection services, 
            Func<IServiceProvider, DbConnection> factory,
            Func<DbLoggingConfigurationBuilder, DbLoggingConfigurationBuilder> config = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var builder = new DbLoggingConfigurationBuilder();
            builder = config?.Invoke(builder) ?? builder;

            object FactoryWrapper(IServiceProvider x) => 
                new ContextfulLoggingFactory<T>(
                x.GetService<ILogger<IDbConnectionFactory<T>>>(), 
                builder.Build(),
                () => factory(x));

            services.TryAdd(new ServiceDescriptor(typeof(IDbConnectionFactory<T>), FactoryWrapper, lifetime));
            return services;
        }
        
        /// <summary>
        /// Registers DbConnectionFactory (with hooks) in the IoC container 
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The connection factory delegate</param>
        /// <param name="lifetime">The service lifetime</param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnectionFactoryWithHooks<T>(
            this IServiceCollection services,
            Func<IServiceProvider, DbConnection> factory,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            object FactoryWrapper(IServiceProvider x) => 
                new WrappedConnectionFactory<T>(() => factory(x));

            services.TryAdd(new ServiceDescriptor(typeof(IHookedDbConnectionFactory<T>), FactoryWrapper, lifetime));
            return services;
        }

        /// <summary>
        /// Extracts the parameter name-value pairs from a DBCommand
        /// </summary>
        /// <param name="command">The DBCommand</param>
        /// <param name="hideValues">Replace values with a mask</param>
        /// <returns>Parameter values by names</returns>
        public static Dictionary<string, object> GetParameters(this DbCommand command, bool hideValues = false)
        {
            IEnumerable<DbParameter> GetParameters()
            {
                foreach (DbParameter parameter in command.Parameters)
                    yield return parameter;
            }

            return GetParameters()
                .ToDictionary(
                    k => k.ParameterName,
                    v => hideValues
                        ? "?"
                        : v.Value == null || v.Value is DBNull
                            ? "<null>"
                            : v.Value);
        }
    }
}