using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Tests.Infra
{
    public class LogMessage
    {
        public LogMessage(
            LogLevel logLevel, 
            EventId eventId, 
            IReadOnlyList<KeyValuePair<string, object>> state, 
            Exception exception, 
            string text)
        {
            LogLevel = logLevel;
            EventId = eventId;
            State = state.ToDictionary(k => k.Key, v => v.Value);
            Exception = exception;
            Text = text;
        }

        public LogLevel LogLevel { get; }
        public EventId EventId { get;  }
        public Dictionary<string,object> State { get; }
        public Exception Exception { get; }
        public string Text { get; }
    }
}