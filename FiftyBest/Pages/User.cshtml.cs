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

    public async Task OnGet()
    {
        if (User.Identity?.Name is null)
            return;

        var userName = User.Identity.Name;
        Restaurants = await dataStore.ReadVisitedRestaurants(userName);
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

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage();
    }
}