
namespace Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using Npgsql;

public interface IDataStore
{
    Task CreateUser(string userName);
    Task<bool> UserExists(string userName);
    
    //Queries for restaurants:
    Task<List<Ranking>> RestaurantsYear(string[] year);
    Task<List<Ranking>> RestaurantsYearCity(string[] year, string city);
    Task<List<Ranking>> RestaurantsYearCountry(string[] year, string country);

    //Queries for cities:
    Task<List<City>> CitiesYearCountry(string[] year, string country);

    //Queries for restaurants:
    Task<List<Country>> CountriesYear(string[] year);
}