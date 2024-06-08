
namespace Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using Npgsql;
using System.Collections.Generic;

public interface IDataStore
{
    Task CreateUser(string userName);
    Task<bool> UserExists(string userName);
    Task<bool> UserHasVisited(string userName, int restaurantId);
    Task UpdateUser(string userName, string newUserName);
    Task AddVisit(string userName, int restaurantId);
    Task RemoveVisit(string userName, int restaurantId);
    Task AddScoreToVisit(string userName, int restaurantId, string score);
    Task<Dictionary<int, string?>> ReadVisits(string userName);
    Task<IReadOnlyCollection<Restaurant>> ReadVisitedRestaurants(string userName);

    //Queries for restaurants:
    Task<Restaurant?> ReadRestaurant(int id);
    Task<IReadOnlyCollection<Ranking>> ReadRankings(int restuarantId);
    Task<List<Ranking>> RestaurantsYear(string[] year);
    Task<List<Ranking>> RestaurantsYearCity(string[] year, string city);
    Task<List<Ranking>> RestaurantsYearCountry(string[] year, string country);

    //Queries for cities:
    Task<List<City>> CitiesYearCountry(string[] year, string country);

    //Queries for restaurants:
    Task<List<Country>> CountriesYear(string[] year);
}