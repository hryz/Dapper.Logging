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
        public static IServiceCollection AddDbConnectionFactory(
            this IServiceCollection services, 
            Func<IServiceProvider, DbConnection> factory,
            Action<DbLoggingConfigurationBuilder> config = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var builder = new DbLoggingConfigurationBuilder();
            config?.Invoke(builder);

            object FactoryWrapper(IServiceProvider x) => new DbConnectionFactory(
                x.GetService<ILogger<DbConnection>>(), 
                () => factory(x),
                builder.Build());

            services.TryAdd(new ServiceDescriptor(typeof(IDbConnectionFactory), FactoryWrapper, lifetime));
            return services;
        }
    }
}