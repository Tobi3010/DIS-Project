﻿
namespace Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using Npgsql;

public interface IDataStore
{
    Task CreateUser(string userName);
    Task<bool> UserExists(string userName);
    
    //Queries for restaurants:
    Task<List<Restaurant>> GetRestaurants(NpgsqlCommand cmd);
    Task<List<Restaurant>> RestaurantsYear(string[] years);
    Task<List<Restaurant>> RestaurantsYearCity(string[] years, string city);
    Task<List<Restaurant>> RestaurantsYearCountry(string[] years, string country);

    //Queries for cities:
    Task<List<City>> GetCities(NpgsqlCommand cmd);
    Task<List<City>> CitiesYearCountry(string[] years, string country);

    //Queries for restaurants:
    Task<List<Country>> GetCountries(NpgsqlCommand cmd);
    Task<List<Country>> CountriesYear(string[] years);

    
}