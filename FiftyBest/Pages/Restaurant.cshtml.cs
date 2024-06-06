using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;

public class RestaurantModel(IDataStore dataStore) : PageModel
{
    public Restaurant? Restaurant { get; set; }

    public IReadOnlyCollection<Ranking> Rankings { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    
    public async Task OnGet()
    {
        Restaurant = await dataStore.ReadRestaurant(Id);
        Rankings = await dataStore.ReadRankings(Id);
    }
}
