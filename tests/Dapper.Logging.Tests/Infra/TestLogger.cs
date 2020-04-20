using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Tests.Infra
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<LogMessage> Messages { get; } = new List<LogMessage>();

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            Messages.Add(new LogMessage(
                logLevel,
                eventId,
                state as IReadOnlyList<KeyValuePair<string, object>>,
                exception,
                formatter(state, exception)));
        }

        public bool IsEnabled(LogLevel logLevel) => true;
        public IDisposable BeginScope<TState>(TState state) => new MemoryStream();
    }
}