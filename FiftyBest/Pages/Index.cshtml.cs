using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
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
    
    private string Year { get; set; }
    private string? Country { get; set; }
   

    public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
        Restaurants = new List<Restaurant>();
        Countries = new List<Country>();
        Cities = new List<City>();
        
        Year = "2023";
        SqlCmd = "SELECT year, rank, restaurantName, cityName FROM Restaurants WHERE year = '"+Year+"';";;
    }

    private void Load()
    {
        Restaurants = _dataStore.GetRestaurants(SqlCmd);
        Countries = _dataStore.GetCountries("SELECT countryName FROM Countries;");
       
    }

    public void OnGet()
    {
        SqlCmd = 
        "SELECT year, rank, restaurantName, cityName FROM Restaurants WHERE year = '"+Year+"';";
        Load();
    }
    public IActionResult OnPostYear(string year)
    {
        Year = year;
        SqlCmd = 
        "SELECT year, rank, restaurantName, cityName FROM Restaurants " 
        +"WHERE year = '"+Year+"';";
        Load();
        return Page();
    }

    public IActionResult OnPostCityButton(string city)
    {
        SqlCmd = 
        "SELECT year, rank, restaurantName, cityName FROM Restaurants "
        +"WHERE year = '"+Year+"' AND cityName = '"+city+"';";
        Load();
        return Page();
    }

    public IActionResult OnPostCountryButton(string country)
    {
        Country = country;
        SqlCmd = 
        "SELECT R.year, R.rank, R.restaurantName, R.cityName FROM Restaurants R " 
        +"JOIN Cities C ON R.cityName = C.cityName "
        +"WHERE R.year = '"+Year+"' AND C.countryName = '"+Country+"';";
        Load();
        return Page();
    }
}
