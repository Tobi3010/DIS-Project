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
    private async Task<List<Restaurant>> GetRestaurants(NpgsqlCommand cmd)
    {
        var restaurants = new List<Restaurant>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            string year = reader.GetString(0);
            string rank = reader.GetString(1); 
            string name = reader.GetString(2); 
            string city = reader.GetString(3);
            restaurants.Add(new Restaurant(year, rank, name, city));
        }
        return restaurants;
    }
    public async Task<List<Restaurant>> RestaurantsYear(string year)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT year, rank, restaurantName, cityName FROM Restaurants"+
            " WHERE year = @year;");
        cmd.Parameters.AddWithValue("year", year); 
        return await GetRestaurants(cmd);
    }

    public async Task<List<Restaurant>> RestaurantsYearCity(string year, string city)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT year, rank, restaurantName, cityName FROM Restaurants "+
            "WHERE year = @year AND cityName = @city;");
        cmd.Parameters.AddWithValue("year", year); 
        cmd.Parameters.AddWithValue("city", city);
        return await GetRestaurants(cmd);
    }

    public async Task<List<Restaurant>> RestaurantsYearCountry(string year, string country)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT R.year, R.rank, R.restaurantName, R.cityName FROM Restaurants R "+
            "JOIN Cities C ON R.cityName = C.cityName "+
            "WHERE R.year = @year AND C.countryName = @country;");
        cmd.Parameters.AddWithValue("year", year); 
        cmd.Parameters.AddWithValue("country", country); 
        return await GetRestaurants(cmd);
    }

    //Queries for Cites
    private async Task<List<City>> GetCities(NpgsqlCommand cmd)
    {
        var cities = new List<City>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            string cityName = reader.GetString(0);
            string countryName = reader.GetString(1); 
            cities.Add(new City(cityName, countryName));
        }
        return cities;
    }

    public async Task<List<City>> CitiesYearCountry(string year, string country)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT DISTINCT C.cityName, C.countryName FROM Cities C "+ 
            "JOIN Restaurants R ON R.cityName = C.cityName "+
            "WHERE R.year = @year AND countryName = @country;"); 
            cmd.Parameters.AddWithValue("year", year);
        cmd.Parameters.AddWithValue("country", country);
        return await GetCities(cmd);
    }

    //Queries for Countries
    private async Task<List<Country>> GetCountries(NpgsqlCommand cmd)
    {
        var countries = new List<Country>();
        await using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            string countryName = reader.GetString(0); 
            countries.Add(new Country(countryName));
        }
        return countries;
    }

    public async Task<List<Country>> CountriesYear(string year)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var cmd = dataSource.CreateCommand(
            "SELECT DISTINCT C.countryName FROM Countries C "+
            "JOIN Cities Ci ON C.countryName = Ci.countryName "+
            "JOIN Restaurants R ON R.cityName = Ci.cityName "+
            "WHERE R.year = @year;"); 
        cmd.Parameters.AddWithValue("year", year); 
        return await GetCountries(cmd);
    }
}