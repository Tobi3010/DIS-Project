using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class IndexModel: PageModel
{
    
    private readonly ILogger<IndexModel> _logger;
    private readonly IDataStore _dataStore;
    public List<Restaurant> Restaurants { get; set; }
    public List<Country> Countries { get; set; }
    public List<City> Cities { get; set; }
    private string SqlCmd { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
        Restaurants = new List<Restaurant>();
        Countries = new List<Country>();
        Cities = new List<City>();
        
        SqlCmd = "SELECT year, rank, restaurantName, cityName FROM Restaurants;";

    }

    public void OnGet()
    {
        Restaurants = _dataStore.GetRestaurants(SqlCmd);
        Countries = _dataStore.GetCountries("SELECT countryName FROM Countries;");
    }
     public async Task<IActionResult> OnPostYear(string year)
    {
        await _dataStore.InsertData(year);
        return RedirectToPage();
    }

    public IActionResult OnPostCityButton(string city)
    {
        SqlCmd = 
        "SELECT year, rank, restaurantName, cityName FROM Restaurants WHERE cityName = '"+city+"';";
        Restaurants = _dataStore.GetRestaurants(SqlCmd);
        return Page();
    }

    public IActionResult OnPostCountryButton(string country)
        {
            SqlCmd = 
            "SELECT R.year, R.rank, R.restaurantName, R.cityName FROM Restaurants R " 
            +"JOIN Cities C ON R.cityName = C.cityName WHERE C.countryName = '"+country+"';";
            Restaurants = _dataStore.GetRestaurants(SqlCmd);
            return Page();
        }
}
