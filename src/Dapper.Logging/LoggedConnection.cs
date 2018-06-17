using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    public static class Extensions
    {
        public static DbConnection WithLog(this DbConnection src, ILogger<DbCommand> logger)
        {
            return new LoggedConnection(src, logger);
        }
    }

    internal class LoggedConnection : DbConnection
    {
        private readonly DbConnection _connection;
        private readonly ILogger<DbCommand> _logger;

        public LoggedConnection(DbConnection connection, ILogger<DbCommand> logger)
        {
            _connection = connection;
            _connection = connection;
            _logger = logger;
        }


        public override void Close()
        {
            var sw = Stopwatch.StartNew();
            _connection.Close();
            _logger.LogInformation("Dapper connection: close, elapsed: {0} ms", sw.ElapsedMilliseconds);
        }

        public override void Open()
        {
            var sw = Stopwatch.StartNew();
            _connection.Open();
            _logger.LogInformation("Dapper connection: open, elapsed: {0} ms", sw.ElapsedMilliseconds);
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Dapper connection: open async, elapsed: {0} ms", sw.ElapsedMilliseconds);
        }

        protected override DbCommand CreateDbCommand() => new LoggedCommand(_connection.CreateCommand(), this, _logger);

        //other members

        public override string ConnectionString
        {
            get => _connection.ConnectionString;
            set => _connection.ConnectionString = value;
        }

        public override int ConnectionTimeout => _connection.ConnectionTimeout;
        public override string Database => _connection.Database;
        public override string DataSource => _connection.DataSource;
        public override string ServerVersion => _connection.ServerVersion;
        public override ConnectionState State => _connection.State;
        public override void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => _connection.BeginTransaction(isolationLevel);
        protected override bool CanRaiseEvents => false;
        public override void EnlistTransaction(System.Transactions.Transaction transaction) => _connection.EnlistTransaction(transaction);
        public override DataTable GetSchema() => _connection.GetSchema();
        public override DataTable GetSchema(string collectionName) => _connection.GetSchema(collectionName);
        public override DataTable GetSchema(string collectionName, string[] restrictionValues) => _connection.GetSchema(collectionName, restrictionValues);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _connection?.Dispose();

            base.Dispose(disposing);
        }
    }
}
