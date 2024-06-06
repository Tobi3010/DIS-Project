using Npgsql;
using Microsoft.Net.Http;
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

    //Queries for restaurants
    private static async Task<List<Restaurant>> GetRestaurants(NpgsqlCommand cmd)
    {
        var restaurants = new List<Restaurant>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            int year = (int)reader["year"];
            int rank = (int)reader["rank"];
            string name = (string)reader["restaurantName"];
            string city = (string)reader["cityName"];
            restaurants.Add(new Restaurant(year, rank, name, city));
        }
        return restaurants;
    }
    public async Task<List<Restaurant>> RestaurantsYear(string[] years)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand();
        cmd.CommandText =
            "SELECT Ranks.year, Ranks.rank, Rs.restaurantName, Rs.cityName FROM Ranks " +
            "JOIN Restaurants Rs ON Ranks.restaurantId = Rs.id " +
            $"WHERE Ranks.year IN ({YearsToSQL(years, cmd)}) " +
            "ORDER BY Ranks.rank ASC, Ranks.year DESC;";
        return await GetRestaurants(cmd);
    }

     public async Task<List<Restaurant>> RestaurantsYearCity(string[] years, string city)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand();
        cmd.CommandText =
            "SELECT Ranks.year, Ranks.rank, Rs.restaurantName, Rs.cityName FROM Ranks " +
            "JOIN Restaurants Rs ON Ranks.restaurantId = Rs.id " +
            $"WHERE Ranks.year IN ({YearsToSQL(years, cmd)}) AND Rs.cityName = @city "+
            "ORDER BY Ranks.rank ASC, Ranks.year DESC;";
        cmd.Parameters.AddWithValue("city", city);
        return await GetRestaurants(cmd);
    }

    public async Task<List<Restaurant>> RestaurantsYearCountry(string[] years, string country)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand();
        cmd.CommandText =
            "SELECT Ranks.year, Ranks.rank, Rs.restaurantName, Rs.cityName FROM Ranks " +
            "JOIN Restaurants Rs ON Ranks.restaurantId = Rs.id " +
            "JOIN Cities C ON Rs.cityName = C.cityName "+
            $"WHERE Ranks.year IN ({YearsToSQL(years, cmd)}) AND C.countryName = @country "+
            "ORDER BY Ranks.rank ASC, Ranks.year DESC;";
        cmd.Parameters.AddWithValue("country", country); 
        return await GetRestaurants(cmd);
    }

    //Queries for Cites
    private static async Task<List<City>> GetCities(NpgsqlCommand cmd)
    {
        var cities = new List<City>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            string cityName = (string)reader["cityName"];
            string countryName = (string)reader["countryName"];
            cities.Add(new City(cityName, countryName));
        }
        return cities;
    }

    public async Task<List<City>> CitiesYearCountry(string[] years, string country)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand();
        cmd.CommandText =
            "SELECT DISTINCT C.cityName, C.countryName FROM Cities C " +
            "JOIN Restaurants Rs ON Rs.cityName = C.cityName "+
            "JOIN Ranks ON Rs.id = Ranks.restaurantId "+
            $"WHERE Ranks.year IN ({YearsToSQL(years, cmd)}) AND C.countryName = @country;"; 
        cmd.Parameters.AddWithValue("country", country);
        return await GetCities(cmd);
    }

    //Queries for Countries
    private static async Task<List<Country>> GetCountries(NpgsqlCommand cmd)
    {
        var countries = new List<Country>();
        await using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            string countryName = reader.GetString(0); 
            countries.Add(new Country(countryName));
        }
        return countries;
    }

    public async Task<List<Country>> CountriesYear(string[] years)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand();
        cmd.CommandText =
            "SELECT DISTINCT C.countryName FROM Countries C "+
            "JOIN Cities Ci ON C.countryName = Ci.countryName "+
            "JOIN Restaurants Rs ON Rs.cityName = Ci.cityName "+
            "JOIN Ranks ON Rs.id = Ranks.restaurantId "+
            $"WHERE Ranks.year IN ({YearsToSQL(years, cmd)});"; 
        return await GetCountries(cmd);
    }

    private string YearsToSQL(string[] years, NpgsqlCommand cmd)
    {
        if (years.Length == 0) {  return "0000"; } // In case of invalid input
        List<string> names = new List<string>();
        for (int i = 0; i < years.Length; i++) {
            names.Add($"@year{i}");
            cmd.Parameters.AddWithValue($"@year{i}", int.Parse(years[i]));
        }
        return string.Join(",", names);
    }



}
