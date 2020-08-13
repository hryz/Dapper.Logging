﻿using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Logging.Hooks
{
    internal class WrappedCommand<T> : DbCommand
    {
        private readonly DbCommand _command;
        private DbConnection _connection;
        private readonly ISqlHooks<T> _hooks;
        private readonly T _context;

        public WrappedCommand(
            DbCommand command,
            DbConnection connection,
            ISqlHooks<T> hooks,
            T context)
        {
            _command = command;
            _connection = connection;
            _hooks = hooks;
            _context = context;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            var start = Stopwatch.GetTimestamp();
            var reader = _command.ExecuteReader(behavior);
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return reader;
        }

        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(
            CommandBehavior behavior, CancellationToken cancellationToken)
        {
            var start = Stopwatch.GetTimestamp();
            var reader = await _command.ExecuteReaderAsync(behavior, cancellationToken);
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return reader;
        }

        public override int ExecuteNonQuery()
        {
            var start = Stopwatch.GetTimestamp();
            var result = _command.ExecuteNonQuery();
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return result;
        }

        public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            var start = Stopwatch.GetTimestamp();
            var result = await _command.ExecuteNonQueryAsync(cancellationToken);
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return result;
        }

        public override object ExecuteScalar()
        {
            var start = Stopwatch.GetTimestamp();
            var result = _command.ExecuteScalar();
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return result;
        }

        public override async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            var start = Stopwatch.GetTimestamp();
            var result = await _command.ExecuteScalarAsync(cancellationToken);
            _hooks.CommandExecuted(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
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

        double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}