using Microsoft.AspNetCore.Mvc;

namespace SpotifyDecisionHelper.Controllers;

public class StatsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}