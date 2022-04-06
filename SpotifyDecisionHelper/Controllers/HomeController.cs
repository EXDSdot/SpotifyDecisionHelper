using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var loginRequest = new LoginRequest(
            new Uri("http://localhost:8888/callback"),
            "4fd4e9e924e2407683824eb05842d1a5",
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
        };

        Uri uri = loginRequest.ToUri();
        return Redirect(uri.ToString());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
