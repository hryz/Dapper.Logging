using System.Data.Common;
using Dapper.Logging.Configuration;
using Dapper.Logging.Tests.Infra;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Dapper.Logging.Tests
{
    public class ServiceRegistrationTests
    {
        [Fact]
        public void Fluent_api_registration_should_work()
        {
            var logger = new TestLogger<IDbConnectionFactory>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<IDbConnectionFactory>>(logger);

            services.AddDbConnectionFactory(
                prv => innerConnection,
                x => x.WithLogLevel(LogLevel.Information),
                ServiceLifetime.Singleton);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Open();
            innerConnection.Received().Open();
            logger.Messages.Should().HaveCount(1);
        }
        
        [Fact]
        public void Manual_creation_of_builder_should_work()
        {
            var logger = new TestLogger<IDbConnectionFactory>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<IDbConnectionFactory>>(logger);
            
            services.AddDbConnectionFactory( 
                prv => innerConnection,
                x => new DbLoggingConfigurationBuilder //discard arg and create a new one
                {
                    LogLevel = LogLevel.Information,
                    LogSensitiveData = true
                }, 
                ServiceLifetime.Singleton);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Open();
            innerConnection.Received().Open();
            logger.Messages.Should().HaveCount(1);
        }
        
        [Fact]
        public void Null_delegate_builder_should_work()
        {
            var logger = new TestLogger<IDbConnectionFactory>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<IDbConnectionFactory>>(logger);
            
            services.AddDbConnectionFactory(prv => innerConnection);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Open();
            innerConnection.Received().Open();
            logger.Messages.Should().HaveCount(1);
        }
        
        [Fact]
        public void Delegate_returning_null_should_work()
        {
            var logger = new TestLogger<IDbConnectionFactory>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<IDbConnectionFactory>>(logger);
            
            services.AddDbConnectionFactory(
                prv => innerConnection,
                _ => null);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Open();
            innerConnection.Received().Open();
            logger.Messages.Should().HaveCount(1);
        }
    }
}