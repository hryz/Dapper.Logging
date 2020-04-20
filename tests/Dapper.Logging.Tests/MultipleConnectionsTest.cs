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
    public class MultipleConnectionsTest
    {
        [Fact]
        public void Should_support_multiple_connections()
        {
            var logger = new TestLogger<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<DbConnection>>(logger);
            
            var con1 = Substitute.For<DbConnection>();
            var con2 = Substitute.For<DbConnection>();

            // Connection 1
            services.AddTransient<IConnectionFactory1>(prv =>
                new ConnectionFactory1(
                    new ContextlessLoggingFactory(
                        prv.GetRequiredService<ILogger<DbConnection>>(),
                        new DbLoggingConfigurationBuilder(),
                        () => con1)));

            // Connection 2
            services.AddTransient<IConnectionFactory2>(prv =>
                new ConnectionFactory2(
                    new ContextlessLoggingFactory(
                        prv.GetRequiredService<ILogger<DbConnection>>(),
                        new DbLoggingConfigurationBuilder(),
                        () => con2)));
            
            var provider = services.BuildServiceProvider();
            var factory1 = provider.GetRequiredService<IConnectionFactory1>();
            var factory2 = provider.GetRequiredService<IConnectionFactory2>();
            
            var connection = factory1.CreateConnection();
            var connection2 = factory2.CreateConnection();
            connection.Open();
            connection2.Open();
            
            //Asserts
            con1.Received().Open();
            con2.Received().Open();
            logger.Messages.Should().HaveCount(2);
        }
    }
    
    
    public interface IConnectionFactory1 : IDbConnectionFactory { }
    public class ConnectionFactory1 : IConnectionFactory1
    {
        private readonly IDbConnectionFactory _inner;
        public ConnectionFactory1(IDbConnectionFactory inner) => _inner = inner;
        public DbConnection CreateConnection() => _inner.CreateConnection();
    }

    public interface IConnectionFactory2 : IDbConnectionFactory { }
    public class ConnectionFactory2 : IConnectionFactory2
    {
        private readonly IDbConnectionFactory _inner;
        public ConnectionFactory2(IDbConnectionFactory inner) => _inner = inner;
        public DbConnection CreateConnection() => _inner.CreateConnection();
    }
}