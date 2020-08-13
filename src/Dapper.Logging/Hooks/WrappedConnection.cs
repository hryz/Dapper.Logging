using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Logging.Hooks
{
    internal class WrappedConnection<T> : DbConnection
    {
        private readonly DbConnection _connection;
        private readonly ISqlHooks<T> _hooks;
        private readonly T _context;

        public WrappedConnection(DbConnection connection, ISqlHooks<T> hooks, T context)
        {
            _connection = connection;
            _hooks = hooks;
            _context = context;
        }

        public override void Close()
        {
            var start = Stopwatch.GetTimestamp();
            _connection.Close();
            _hooks.ConnectionClosed(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
        }

        public override void Open()
        {
            var start = Stopwatch.GetTimestamp();
            _connection.Open();
            _hooks.ConnectionOpened(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            var start = Stopwatch.GetTimestamp();
            await _connection.OpenAsync(cancellationToken);
            _hooks.ConnectionOpened(this, _context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
        }

        protected override DbCommand CreateDbCommand() => 
            new WrappedCommand<T>(_connection.CreateCommand(), this, _hooks, _context);

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

        double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
