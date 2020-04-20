using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper.Logging.Configuration;
using Dapper.Logging.Tests.Infra;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Dapper.Logging.Tests
{
    public class LoggingTests
    {
        [Fact]
        public void Should_log_opening_of_connection()
        {
            var logger = new TestLogger<DbConnection>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<DbConnection>>(logger);

            services.AddDbConnectionFactory(
                prv => innerConnection,
                x => x.WithLogLevel(LogLevel.Information),
                ServiceLifetime.Singleton);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Open();
            
            //assert
            innerConnection.Received().Open();
            logger.Messages.Should().HaveCount(1);
            logger.Messages[0].Text.Should().Contain("open");
        }
        
        [Fact]
        public void Should_log_closing_of_connection()
        {
            var logger = new TestLogger<DbConnection>();
            var innerConnection = Substitute.For<DbConnection>();
            var services = new ServiceCollection()
                .AddSingleton<ILogger<DbConnection>>(logger);

            services.AddDbConnectionFactory(
                prv => innerConnection,
                x => x.WithLogLevel(LogLevel.Information),
                ServiceLifetime.Singleton);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            connection.Close();
            
            //assert
            innerConnection.Received().Close();
            logger.Messages.Should().HaveCount(1);
            logger.Messages[0].Text.Should().Contain("close");
        }
        
        [Fact]
        public void Should_log_queries()
        {
            var logger = new TestLogger<DbConnection>();
            var innerConnection = Substitute.For<DbConnection>();
            var innerCmd = Substitute.For<DbCommand>();
            var innerParams = Substitute.For<DbParameterCollection>();
            var param = Substitute.For<DbParameter>();
            param.ParameterName.Returns("@id");
            param.Value.Returns("1");
            innerParams.GetEnumerator().Returns(new []{param}.GetEnumerator());
            innerCmd.Parameters.Returns(innerParams);
            innerConnection.CreateCommand().Returns(innerCmd);
            var services = new ServiceCollection()
                .AddSingleton<ILogger<DbConnection>>(logger);

            services.AddDbConnectionFactory(
                prv => innerConnection,
                x => x.WithLogLevel(LogLevel.Information)
                    .WithSensitiveDataLogging(),
                ServiceLifetime.Singleton);

            var provider = services.BuildServiceProvider();
            var factory = provider.GetRequiredService<IDbConnectionFactory>();
            
            var connection = factory.CreateConnection();
            var cmd = connection.CreateCommand();
            cmd.ExecuteNonQuery();
            
            //assert
            innerConnection.Received().CreateCommand();
            innerCmd.Received().ExecuteNonQuery();
            
            logger.Messages.Should().HaveCount(1);
            logger.Messages[0].Text.Should().Contain("query");
            logger.Messages[0].State.Should().ContainKey("params");
            logger.Messages[0].State["params"].Should().BeOfType<string>();
            logger.Messages[0].State["params"].ToString().Should().Contain("id");
            logger.Messages[0].State["params"].ToString().Should().Contain("1");
        }
    }
}