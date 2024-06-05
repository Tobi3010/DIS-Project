using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;


namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDataStore _dataStore;
    public List<Restaurant> Restaurants { get; set; }
    public List<Country> Countries { get; set; }
    public List<City> Cities { get; set; }
    [BindProperty] public string Years { get; set; }
    [BindProperty] public string? Country { get; set; }
    public string? Error { get; set; } = null;

    public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
        Restaurants = new List<Restaurant>();
        Countries = new List<Country>();
        Cities = new List<City>();
        Years = "2023";
    }
    private async Task Load()
    {
        Restaurants = await _dataStore.RestaurantsYear(validate(Years));
        Countries = await _dataStore.CountriesYear(validate(Years));
    }
    public async Task OnGet()
    {
        await Load();
    }
    public async Task<IActionResult> OnPostYear(string years)
    {
        await Load();
        Years = years;
        return Page();
    }
    public async Task<IActionResult> OnPostCityButton(string city)
    {
        await Load();
        Restaurants = await _dataStore.RestaurantsYearCity(validate(Years), city);
        if (Country != null) Cities = await _dataStore.CitiesYearCountry(validate(Years), Country);
        //Make sure the selected city appears first in the menu
        int idx = Cities.FindIndex(x => x.Name == city);
        (Cities[idx], Cities[0]) = (Cities[0], Cities[idx]); //swap
        return Page();
    }
    public async Task<IActionResult> OnPostCountryButton(string country)
    {
        await Load();
        Country = country;
        Restaurants = await _dataStore.RestaurantsYearCountry(validate(Years), Country);
        Cities = await _dataStore.CitiesYearCountry(validate(Years), Country);
        //Make sure the selected country appears first in the menu
        int idx = Countries.FindIndex(x => x.Name == Country);
        (Countries[idx], Countries[0]) = (Countries[0], Countries[idx]); //swap

        return Page();
    }
    private string[] validate(string input)
    {
        string yearPattern = "(200[2-9]|201[0-9]|202[1-3])"; // Years 2002-2023, exept 2020
        //                             One year        Year List       Year Interval
        Regex pattern = new Regex($"^{yearPattern}((,{yearPattern})*|(-{yearPattern})?)$");
        string errorMessage = 
            "Invalid request, follow the rules : \n"+ 
            "years from 2002 to 2023, exept 2020. \n"+
            "lists : year1,year2,year3,... "+
            "intervals : year1-year2 where year2 > year1";
        if (pattern.IsMatch(input)) {
            if (input.Contains("-")) { // Handles interval between two years
                string[] parts = input.Split("-");
                int year1 = int.Parse(parts[0]);
                int year2 = int.Parse(parts[1]);
                if (year1 <= year2){ // Ensure year2 > year1, cant do that in regex
                    // Makes int[] of values between year1 and year2, and convert to string
                    int[] arr = Enumerable.Range(year1, year2-year1+1).ToArray();
                    return Array.ConvertAll(arr, x => x.ToString());
                } else {
                    Error = errorMessage;
                    return [];
                }
            } else { // Handles just one year, or list of years
                return input.Split(",");
            }
        } else {
            Error = errorMessage;
            return [];
        }
    }


}
