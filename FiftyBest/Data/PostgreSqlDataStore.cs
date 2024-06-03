using Npgsql;
using Setoma.CompSci.Dis.FiftyBest.Models;

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

    public List<Restaurant> GetRestaurants(string sqlCmd)
    {
        var restaurants = new List<Restaurant>();

        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var connection = dataSource.CreateConnection();
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sqlCmd;

        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            string year = reader.GetString(0);
            string rank = reader.GetString(1); 
            string name = reader.GetString(2); 
            string city = reader.GetString(3);
            restaurants.Add(new Restaurant(year, rank, name, city));
        }
        return restaurants;
    }

    public List<City> GetCities(string sqlCmd)
    {
        var cities = new List<City>();

        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var connection = dataSource.CreateConnection();
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sqlCmd;

        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            string cityName = reader.GetString(0);
            string countryName = reader.GetString(1); 
            cities.Add(new City(cityName, countryName));
        }
        return cities;
    }

    public List<Country> GetCountries(string sqlCmd)
    {
        var countries = new List<Country>();

        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var connection = dataSource.CreateConnection();
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sqlCmd;

        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            string countryName = reader.GetString(0); 
            countries.Add(new Country(countryName));
        }
        return countries;
    }


    
        



}
