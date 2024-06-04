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
    [BindProperty] public string Year { get; set; }
    [BindProperty] public string? Country { get; set; }
   

    public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
        Restaurants = new List<Restaurant>();
        Countries = new List<Country>();
        Cities = new List<City>();
        Year = "2023";
    }

    private async Task Load()
    {
        Restaurants = await _dataStore.RestaurantsYear(Year);
        Countries = await _dataStore.CountriesYear(Year);
    }

    public async Task OnGet()
    {
        await Load();
       
    }
    public async Task<IActionResult> OnPostYear(string year)
    {
        await Load();
        Year = year;
        return Page();
    }

    public async Task<IActionResult> OnPostCityButton(string city)
    {
        await Load();
        Restaurants = await _dataStore.RestaurantsYearCity(Year, city);
        if (Country != null) Cities = await _dataStore.CitiesYearCountry(Year, Country);
         //Make sure the selected city appears first in the menu
        int idx = Cities.FindIndex(x => x.Name == city); 
        (Cities[idx], Cities[0]) = (Cities[0], Cities[idx]); //swap
        return Page();
    }

    public async Task<IActionResult> OnPostCountryButton(string country)
    {
        await Load();
        Country = country;
        if (Country != null) {
            Restaurants = await _dataStore.RestaurantsYearCountry(Year, Country);
            Cities = await _dataStore.CitiesYearCountry(Year, Country);
            //Make sure the selected country appears first in the menu
            int idx = Countries.FindIndex(x => x.Name == Country); 
            (Countries[idx], Countries[0]) = (Countries[0], Countries[idx]); //swap
        }
        return Page();
    }
}
