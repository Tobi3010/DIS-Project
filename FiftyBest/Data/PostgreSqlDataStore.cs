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

    public async Task InsertData(string fromYear)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var cmdDelete = dataSource.CreateCommand(
            "DELETE FROM Restaurants; DELETE FROM Cities; DELETE FROM Countries;"); // Delete the previous list
        await cmdDelete.ExecuteNonQueryAsync();

        string path = Path.Combine("..", "Data", fromYear+".csv");
        using var reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrEmpty(line))
            {
                var parts = line.Split(','); //split line of .csv into parts separated by ","
                // We can then access the different parts as seen below:
                using var cmdInsert1 = dataSource.CreateCommand(
                    "INSERT INTO Restaurants (Year, rank, restaurantName, cityName) "
                    +"VALUES (@year, @rank, @restaurantName, @cityName);");
                cmdInsert1.Parameters.AddWithValue("year", fromYear); 
                cmdInsert1.Parameters.AddWithValue("rank", parts[0]); 
                cmdInsert1.Parameters.AddWithValue("restaurantName", parts[1]);
                cmdInsert1.Parameters.AddWithValue("cityName", parts[2]); 
                await cmdInsert1.ExecuteNonQueryAsync();

                using var cmdInsert2 = dataSource.CreateCommand(
                    "INSERT INTO Cities (cityName, countryName) VALUES (@cityName, @countryName);");
                cmdInsert2.Parameters.AddWithValue("cityName", parts[2]); 
                cmdInsert2.Parameters.AddWithValue("countryName", parts[3]);
                await cmdInsert2.ExecuteNonQueryAsync();

                 using var cmdInsert3 = dataSource.CreateCommand(
                    "INSERT INTO Countries (countryName) VALUES (@countryName);");
                cmdInsert3.Parameters.AddWithValue("countryName", parts[3]);
                await cmdInsert3.ExecuteNonQueryAsync(); 
            }
        }
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
