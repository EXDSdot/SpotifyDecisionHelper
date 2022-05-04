using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyDecisionHelper.Models;
using System.IO;

namespace SpotifyDecisionHelper.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private SpotifyClient spotify;
    
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
        return View();
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
    
    public IActionResult Callback([FromQuery] string code)
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
        spotify = new SpotifyClient(response.Result.AccessToken);
        
        LogLibrary();
        
        return Redirect("https://localhost:7142/");
    }

    public async void LogLibrary()
    {
        var user = await spotify.UserProfile.Current();
        var tracks = await spotify.Library.GetTracks();
        using(StreamWriter sw = new StreamWriter(user.Id+".txt"))
        {
            await foreach (var track in spotify.Paginate(tracks))
            {
                sw.WriteLine(track.Track.Artists[0].Name+" - "+track.Track.Album.Name+" - "+track.Track.Name);
            }
        }

    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
