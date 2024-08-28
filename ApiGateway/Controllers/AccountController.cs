using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Welcome to API Gateway");
    }

    [Authorize]
    [HttpGet("login")]
    public IActionResult Login()
    {
        var referer = HttpContext.Request.Headers.Referer.ToString();
        var redirectUri = string.IsNullOrEmpty(referer) ? "https://localhost:4200" : referer;
        return Redirect(redirectUri);
    }
    
    [AllowAnonymous]
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        var claims = HttpContext.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
        return Ok(claims);
    }
    
    [HttpGet("logout")]
    public async Task Logout()
    {
        var prop = new AuthenticationProperties
        {
            RedirectUri = "/account/public"
        };
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, prop);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}