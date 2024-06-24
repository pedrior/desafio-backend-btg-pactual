using System.Data;
using Npgsql;

namespace BtgPactual.OrderProcessor.Persistence;

public sealed class DbSession : IDisposable
{
    public DbSession(string connectionString)
    {
        Connection = new NpgsqlConnection(connectionString);
        Connection.Open();
    }

    public IDbConnection Connection { get; }
    
    public IDbTransaction? Transaction { get; private set; }

    public void Dispose() => Transaction?.Dispose();
    
    public void BeginTransaction() => Transaction = Connection.BeginTransaction();
    
    public void CommitTransaction()
    {
        Transaction?.Commit();
        Transaction = null;
        
        Dispose();
    }
    
    public void RollbackTransaction()
    {
        Transaction?.Rollback();
        Transaction = null;
        
        Dispose();
    }
}