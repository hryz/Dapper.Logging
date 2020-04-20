using System;
using Dapper.Logging.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Dapper.Logging.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void Manually_created_configuration_should_be_valid()
        {
            var cfg = new DbLoggingConfiguration(
                LogLevel.None,
                null,
                null,
                null);

            cfg.OpenConnectionMessage.Should().NotBeNullOrEmpty();
            cfg.CloseConnectionMessage.Should().NotBeNullOrEmpty();
            cfg.ExecuteQueryMessage.Should().NotBeNullOrEmpty();
            cfg.ConnectionProjector.Should().NotBeNull();
        }

        [Fact]
        public void Empty_configuration_builder_should_be_valid()
        {
            var builder = new DbLoggingConfigurationBuilder();
            builder.OpenConnectionMessage.Should().NotBeNullOrEmpty();
            builder.CloseConnectionMessage.Should().NotBeNullOrEmpty();
            builder.ExecuteQueryMessage.Should().NotBeNullOrEmpty();
            builder.LogLevel.Should().NotBeNull();
            builder.LogSensitiveData.Should().HaveValue();
            builder.ConnectionProjector.Should().NotBeNull();
        }
        
        [Fact]
        public void Configuration_builder_full_of_nulls_should_be_valid()
        {
            var builder = new DbLoggingConfigurationBuilder
            {
                ConnectionProjector = null,
                LogLevel = null,
                CloseConnectionMessage = null,
                ExecuteQueryMessage = null,
                LogSensitiveData = null,
                OpenConnectionMessage = null
            };
            var cfg = builder.Build();
            cfg.OpenConnectionMessage.Should().NotBeNullOrEmpty();
            cfg.CloseConnectionMessage.Should().NotBeNullOrEmpty();
            cfg.ExecuteQueryMessage.Should().NotBeNullOrEmpty();
            cfg.LogLevel.Should().Be(LogLevel.Information);
            cfg.LogSensitiveData.Should().BeFalse();
            cfg.ConnectionProjector.Should().NotBeNull();
        }
    }
}
