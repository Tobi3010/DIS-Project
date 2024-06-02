using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.Security.Claims;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class IndexModel: PageModel
{
    
    private readonly ILogger<IndexModel> _logger;
    private readonly IDataStore _dataStore;
    public List<Restaurant> Restaurants { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
        Restaurants = new List<Restaurant>();
    }


    public void OnGet()
    {
        var sqlCmd = "SELECT year, rank, restaurantName, cityName FROM Restaurants";
        Restaurants = _dataStore.GetRestaurants(sqlCmd);
    }
    public async Task<IActionResult> OnPostButton2023()
    {
        await _dataStore.InsertData("2023");
        return RedirectToPage();
    }
     public async Task<IActionResult> OnPostButton2022()
    {
        await _dataStore.InsertData("2022");
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostButton2021()
    {
        await _dataStore.InsertData("2021");
        return RedirectToPage();
    }

}
