using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Logging.Configuration;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    internal class LoggedConnection : DbConnection
    {
        private readonly DbConnection _connection;
        private readonly ILogger _logger;
        private readonly DbLoggingConfiguration _cfg;

        public LoggedConnection(DbConnection connection, ILogger logger, DbLoggingConfiguration cfg)
        {
            _connection = connection;
            _logger = logger;
            _cfg = cfg;
        }

        public override void Close()
        {
            var sw = Stopwatch.StartNew();
            _connection.Close();
            sw.Stop();
            _logger.Log(_cfg.LogLevel, _cfg.CloseConnectionMessage, sw.ElapsedMilliseconds);
        }

        public override void Open()
        {
            var sw = Stopwatch.StartNew();
            _connection.Open();
            sw.Stop();
            _logger.Log(_cfg.LogLevel, _cfg.OpenConnectionMessage, sw.ElapsedMilliseconds);
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            await _connection.OpenAsync(cancellationToken);
            sw.Stop();
            _logger.Log(_cfg.LogLevel, _cfg.OpenConnectionMessage, sw.ElapsedMilliseconds);
        }

        protected override DbCommand CreateDbCommand() => new LoggedCommand(_connection.CreateCommand(), this, _logger, _cfg);

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
