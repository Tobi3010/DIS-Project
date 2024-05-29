using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Setoma.CompSci.Dis.FiftyBest.Pages;
public class AuthNModel : PageModel
{
    private readonly ILogger<AuthNModel> _logger;

    public AuthNModel(ILogger<AuthNModel> logger)
    {
        _logger = logger;
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string userName)
    {
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