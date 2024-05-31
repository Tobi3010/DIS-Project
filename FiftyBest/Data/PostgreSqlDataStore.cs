using Npgsql;

namespace Setoma.CompSci.Dis.FiftyBest.Data;

public sealed class PostgreSqlDataStore(string connectionString) : IDataStore
{
    public async Task<bool> UserExists(string userName)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT COUNT(userName) FROM users WHERE userName = ($1)");
        cmd.Parameters.AddWithValue(userName);
        var count = await cmd.ExecuteScalarAsync();
        return count is long i && i > 0;
    }

    public async Task CreateUser(string userName)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "INSERT INTO users (userName) VALUES ($1)");
        cmd.Parameters.AddWithValue(userName);
        await cmd.ExecuteNonQueryAsync();
    }
}
