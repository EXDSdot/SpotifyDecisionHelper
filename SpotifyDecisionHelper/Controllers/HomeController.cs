using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyDecisionHelper.DB.Models;
using SpotifyDecisionHelper.DBLogic.Albums;
using SpotifyDecisionHelper.DBLogic.Artists;
using SpotifyDecisionHelper.DBLogic.Brackets;
using SpotifyDecisionHelper.DBLogic.Matches;
using SpotifyDecisionHelper.DBLogic.Tracks;

namespace SpotifyDecisionHelper.Controllers;

public class HomeController : Controller
{
    private readonly IArtistsManager _artistsManager;
    private readonly IAlbumsManager _albumsManager;
    private readonly ITracksManager _tracksManager;
    private readonly IMatchesManager _matchesManager;
    private readonly IBracketsManager _bracketsManager;
    private SpotifyClient? _spotify;
    
    public HomeController(IArtistsManager artistsManager, IAlbumsManager albumsManager, ITracksManager tracksManager, IBracketsManager bracketsManager, IMatchesManager matchesManager)
    {
        _artistsManager = artistsManager;
        _albumsManager = albumsManager;
        _tracksManager = tracksManager;
        _bracketsManager = bracketsManager;
        _matchesManager = matchesManager;
    }

    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetString("_UserId");
        if (userId == null) View();
        
        var bracketId = _bracketsManager.CurrentBracket(userId);
        var match = _matchesManager.GetNextMatch(userId, bracketId);

        ViewData["MatchSent"] = match != null;
        
        return View(match);
    }
    
    public IActionResult Choice([FromQuery] int matchId, [FromQuery] int choice)
    {
        var userId = HttpContext.Session.GetString("_UserId");
        if (userId == null) return RedirectToAction("Index");

        _matchesManager.ApplyResult(userId, matchId, choice);
        return RedirectToAction("Index");
    }

    public IActionResult Auth()
    {
        var builder = WebApplication.CreateBuilder();
        var loginRequest = new LoginRequest(
            new Uri("https://localhost:7142/Home/Callback"),
            builder.Configuration["Spotify:ClientId"],
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[] { Scopes.UserLibraryRead }
        };

        Uri uri = loginRequest.ToUri();
        return Redirect(uri.ToString());
    }
    
    public async Task<IActionResult> Callback([FromQuery] string code)
    {   
        var builder = WebApplication.CreateBuilder();
        
        var response = new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(
                builder.Configuration["Spotify:ClientId"], 
                builder.Configuration["Spotify:ClientSecret"], 
                code, 
                new Uri("https://localhost:7142/Home/Callback"))
        );
        await response;
        _spotify = new SpotifyClient(response.Result.AccessToken);
        
        HttpContext.Session.SetString("_AToken", response.Result.AccessToken);
        HttpContext.Session.SetString("_RToken", response.Result.RefreshToken);
        HttpContext.Session.SetString("_UserId", (await _spotify.UserProfile.Current()).Id);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Load()
    {
        var token = HttpContext.Session.GetString("_AToken");
        if (token == null) return RedirectToAction("Index");

        _spotify = new SpotifyClient(token);
        var userId = (await _spotify.UserProfile.Current()).Id;
        var tracks = await _spotify.Library.GetTracks();
        
        await foreach (var track in _spotify.Paginate(tracks))
        {
            var fullTrack = track.Track;
            await _artistsManager.Add(userId, fullTrack.Artists[0].Id);
            await _albumsManager.Add(userId, fullTrack.Album.Id, fullTrack.Artists[0].Id);
            await _tracksManager.Add(userId, fullTrack.Id, fullTrack.Album.Id);
        }
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> NewBracket()
    {
        var userId = HttpContext.Session.GetString("_UserId");
        if (userId == null) return RedirectToAction("Index");
        
        await _bracketsManager.AddNewBracket(userId);
        return RedirectToAction("Index");
    }
}
