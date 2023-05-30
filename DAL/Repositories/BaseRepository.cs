using System.Transactions;
using Npgsql;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories;

public abstract class BaseRepository : IDbRepository
{
    protected BaseRepository()
    {
    }

    protected NpgsqlConnection GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection(Constants.ConnectionString);
        connection.Open();
        connection.ReloadTypes();
        return connection;
    }
}