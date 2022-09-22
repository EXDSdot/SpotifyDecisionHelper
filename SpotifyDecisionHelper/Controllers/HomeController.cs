using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyDecisionHelper.DBLogic.Albums;
using SpotifyDecisionHelper.DBLogic.Artists;
using SpotifyDecisionHelper.DBLogic.Tracks;

namespace SpotifyDecisionHelper.Controllers;

public class HomeController : Controller
{
    private readonly IArtistsManager _artistsManager;
    private readonly IAlbumsManager _albumsManager;
    private readonly ITracksManager _tracksManager;
    private SpotifyClient? _spotify;
    
    public HomeController(IArtistsManager artistsManager, IAlbumsManager albumsManager, ITracksManager tracksManager)
    {
        _artistsManager = artistsManager;
        _albumsManager = albumsManager;
        _tracksManager = tracksManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Auth()
    {
        if (_spotify != null) return Redirect("https://localhost:7142/");
        
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
        response.Wait();
        _spotify = new SpotifyClient(response.Result.AccessToken);
        
        await LogLibrary();
        
        return Redirect("https://localhost:7142/");
    }

    public async Task LogLibrary()
    {
        if (_spotify == null) return;

        var userId = (await _spotify.UserProfile.Current()).Id;
        var tracks = await _spotify.Library.GetTracks();
        
        await foreach (var track in _spotify.Paginate(tracks))
        {
            var fullTrack = track.Track;
            await _artistsManager.Add(userId, fullTrack.Artists[0].Id);
            await _albumsManager.Add(userId, fullTrack.Album.Id, fullTrack.Artists[0].Id);
            await _tracksManager.Add(userId, fullTrack.Id, fullTrack.Album.Id);
        }
    }
}
