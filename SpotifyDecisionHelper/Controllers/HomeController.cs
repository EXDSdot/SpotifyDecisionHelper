using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyDecisionHelper.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.Data;

namespace SpotifyDecisionHelper.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private SpotifyClient? _spotify;
    
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
        _spotify = new SpotifyClient(response.Result.AccessToken);
        
        LogLibrary();
        
        return Redirect("https://localhost:7142/");
    }

    public async void LogLibrary()
    {
        if (_spotify == null) return;
        
        var user = await _spotify.UserProfile.Current();
        var tracks = await _spotify.Library.GetTracks();
        /*using (StreamWriter sw = new StreamWriter(user.Id + ".txt"))
        
        var user = await _spotify.UserProfile.Current();
        var tracks = await _spotify.Library.GetTracks();
        using (StreamWriter sw = new StreamWriter(user.Id + ".txt"))
        {
            await foreach (var track in _spotify.Paginate(tracks))
            {
                sw.WriteLine(track.Track.Artists[0].Name + " - " + track.Track.Album.Name + " - " + track.Track.Name);
                sw.WriteLine(track.Track.Artists[0].Name + " - " + track.Track.Album.Name + " - " +
                             track.Track.Name + " - " + track.Track.Popularity);
            }
        }*/

        using (ApplicationContext db = new ApplicationContext())
        {
            //Track curr;
            // Artist _curr; // TODO: Better naming
            await foreach (var track in _spotify.Paginate(tracks))
            {

                //_curr = new Artist(track.Track.Artists[0].Name);
                db.Tracks.Add(new Track(track.Track.Name, track.Track.Artists[0].Name));
                // DbSaveChanges?

            }

            db.SaveChanges(); // SaveChanges vs SaveChanges Async ?
            using (StreamWriter sw = new StreamWriter(user.Id + ".txt"))
            {
                var Tracks = db.Tracks.ToList();
                foreach (var track in Tracks)
                {
                    sw.WriteLine(track.Name + " " + track.Id + " "+ track.ArtistName);
                }
            }
        }
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
