using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;

public class RestaurantModel(IDataStore dataStore) : PageModel
{
    public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;

    [BindProperty, DisplayName("Visited")]
    public bool HasVisited { get; set; }

    public Restaurant? Restaurant { get; set; }

    public IReadOnlyCollection<Ranking> Rankings { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    public string? Error { get; set; } = null;
    
    public async Task OnGet()
    {
        Restaurant = await dataStore.ReadRestaurant(Id);
        Rankings = await dataStore.ReadRankings(Id);
        if (User.Identity?.Name is not null)
            HasVisited = await dataStore.UserHasVisited(
                User.Identity.Name,
                Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity?.Name is null)
            return RedirectToPage();

        if (HasVisited)
            await dataStore.AddVisit(User.Identity.Name, Id);
        else
            await dataStore.RemoveVisit(User.Identity.Name, Id);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostScore(string score)
    {
         if (User.Identity?.Name is null)
            return RedirectToPage();
            
        Regex pattern = new Regex("^[0-5]((?<!5).5)?$");
        if (pattern.IsMatch(score)) {
            await dataStore.AddScoreToVisit(User.Identity.Name, Id, score);
        } 
        return RedirectToPage();
    }
}
