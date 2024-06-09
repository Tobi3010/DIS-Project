using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using Setoma.CompSci.Dis.FiftyBest.Models;
using System.Security.Claims;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class AuthNModel(IDataStore dataStore) : PageModel
{
    public IReadOnlyCollection<Restaurant> Restaurants { get; set; } = [];
    public IReadOnlyCollection<User> Users { get; set; } = [];
    public Dictionary<int, string?> RestaurantScores { get; set; } = new Dictionary<int, string?>();

    public async Task OnGet()
    {
        if (User.Identity?.Name is null)
            return;

        var userName = User.Identity.Name;
        Restaurants = await dataStore.ReadVisitedRestaurants(userName);
        RestaurantScores = await dataStore.ReadVisits(userName);
        Users = await dataStore.ReadUsers(userName);
    }

    public async Task<IActionResult> OnPost(string userName)
    {
        var exists = await dataStore.UserExists(userName);
        if (!exists)
            await dataStore.CreateUser(userName);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(
                new ClaimsIdentity([new Claim(ClaimTypes.Name, userName)],
                CookieAuthenticationDefaults.AuthenticationScheme)));

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostChangeUsername(string userName, string new_userName)
    {
        var exists = await dataStore.UserExists(userName);
        if (exists)
        {
            await dataStore.UpdateUser(userName, new_userName);
        }

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(
                new ClaimsIdentity([new Claim(ClaimTypes.Name, new_userName)],
                CookieAuthenticationDefaults.AuthenticationScheme)));

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage();
    }
}