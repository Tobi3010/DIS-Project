
namespace Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;

public interface IDataStore
{
    Task CreateUser(string userName);
    Task<bool> UserExists(string userName);
    Task InsertData(string fromYear);
    
    List<Restaurant> GetRestaurants(string sqlCmd);
    List<Country> GetCountries(string sqlCmd);
    List<City> GetCities(string sqlCmd);

    
}