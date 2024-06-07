using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Setoma.CompSci.Dis.FiftyBest.Data;
using System.Security.Claims;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class AuthNModel(IDataStore dataStore) : PageModel
{
    public void OnGet()
    {
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