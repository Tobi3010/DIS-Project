using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.ComponentModel;

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
}
