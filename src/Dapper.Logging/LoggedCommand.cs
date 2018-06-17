using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    internal class LoggedCommand : DbCommand
    {
        private readonly DbCommand _command;
        private readonly ILogger<DbCommand> _logger;
        private DbConnection _connection;

        public LoggedCommand(DbCommand command, DbConnection connection, ILogger<DbCommand> logger)
        {
            _command = command;
            _connection = connection;
            _logger = logger;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => 
            ExecuteAndLog(() => _command.ExecuteReader(behavior));

        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken) => 
            ExecuteAndLog(() => _command.ExecuteReaderAsync(behavior, cancellationToken));

        public override int ExecuteNonQuery() => 
            ExecuteAndLog(() => _command.ExecuteNonQuery());

        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken) => 
            ExecuteAndLog(() => _command.ExecuteNonQueryAsync(cancellationToken));

        public override object ExecuteScalar() => 
            ExecuteAndLog(() => _command.ExecuteScalar());

        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken) => 
            ExecuteAndLog(() => _command.ExecuteScalarAsync(cancellationToken));

        private T ExecuteAndLog<T>(Func<T> action)
        {
            var sw = Stopwatch.StartNew();
            var result = action();
            sw.Stop();
            _logger.LogInformation("Dapper query:\r\n{0}, elapsed: {1} ms", CommandText, sw.ElapsedMilliseconds);
            return result;
        }

        private async Task<T> ExecuteAndLog<T>(Func<Task<T>> action)
        {
            var sw = Stopwatch.StartNew();
            var result = await action();
            sw.Stop();
            _logger.LogInformation("Dapper query:\r\n{0}, elapsed: {1} ms", CommandText, sw.ElapsedMilliseconds);
            return result;
        }

        //other members

        public override string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => _command.CommandTimeout;
            set => _command.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => _command.CommandType;
            set => _command.CommandType = value;
        }

        protected override DbConnection DbConnection
        {
            get => _connection;
            set => _connection = value;
        }

        protected override DbTransaction DbTransaction
        {
            get => _command.Transaction;
            set => _command.Transaction = value;
        }

        public override bool DesignTimeVisible
        {
            get => _command.DesignTimeVisible;
            set => _command.DesignTimeVisible = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => _command.UpdatedRowSource;
            set => _command.UpdatedRowSource = value;
        }

        protected override DbParameterCollection DbParameterCollection => _command.Parameters;
        public override void Cancel() => _command.Cancel();
        public override void Prepare() => _command.Prepare();
        protected override DbParameter CreateDbParameter() => _command.CreateParameter();

        protected override void Dispose(bool disposing)
        {
            if (disposing) 
                _command?.Dispose();

            base.Dispose(disposing);
        }
    }
}