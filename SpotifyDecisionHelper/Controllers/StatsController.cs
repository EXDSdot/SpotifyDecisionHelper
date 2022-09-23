using Microsoft.AspNetCore.Mvc;
using SpotifyDecisionHelper.DB.Models;
using SpotifyDecisionHelper.DBLogic.Tracks;

namespace SpotifyDecisionHelper.Controllers;

public class StatsController : Controller
{
    private readonly ITracksManager _tracksManager;

    public StatsController(ITracksManager tracksManager)
    {
        _tracksManager = tracksManager;
    }

   /* public IActionResult Tracks(int? pageNumber)
    {
        
        var tracks = 
        return View(List<Track>);
    }*/
}