using RFService.IServices;
using System.Data;

namespace RFService.Repo
{
    public class RepoOptions
        : IDisposable
    {
        public IDbConnection? Connection { get; private set; }

        public IDbTransaction? Transaction { get; private set; }

        public bool AutoCommit { get; private set; }

        ~RepoOptions()
            => Dispose();

        public void Dispose()
        {
            if (Connection != null)
                Close();

            GC.SuppressFinalize(this);
        }

        public IDbConnection OpenConnection<TEntity>(IService<TEntity> service)
            where TEntity : class
        {
            if (Connection != null)
                throw new Exception("There is another connection opened");

            (Connection, _) = service.OpenConnection();

            return Connection;
        }

        public void Close()
        {
            if (Connection == null)
                throw new Exception("No connection to create a transaction");

            if (Transaction != null)
            {
                if (AutoCommit)
                    Commit();
                else
                    Rollback();
            }

            Connection.Dispose();
            Connection = null;
        }

        public IDbTransaction BeginTransaction<TEntity>(IService<TEntity>? service = null)
            where TEntity : class
        {
            if (Transaction != null)
                throw new Exception("There is anoter transaction in progress");

            if (Connection == null)
            {
                if (service == null)
                    throw new Exception("No connection or service to create a transaction");

                OpenConnection(service);
            }
            else
            {
                if (service == null)
                    throw new Exception("There is a connection service must not be specified");
            }

            Transaction = Connection?.BeginTransaction()
                ?? throw new Exception("Error to open connection for service");

            return Transaction;
        }

        public IDbTransaction BeginAutoCommitTransaction<TEntity>(IService<TEntity>? service = null)
            where TEntity : class
        {
            BeginTransaction(service);
            AutoCommit = true;

            return Transaction
                ?? throw new Exception("Error to begin transaction for service");
        }

        public void Commit()
        {
            if (Transaction == null)
                throw new Exception("No transaction to commit");

            Transaction.Commit();
            Transaction.Dispose();
            Transaction = null;
        }

        public void Rollback()
        {
            if (Transaction == null)
                throw new Exception("No transaction to rollback");

            Transaction.Rollback();
            Transaction.Dispose();
            Transaction = null;
        }
    }
}
