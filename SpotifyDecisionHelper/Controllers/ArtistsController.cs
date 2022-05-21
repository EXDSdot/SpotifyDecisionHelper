using Microsoft.AspNetCore.Mvc;
using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.DBLogic.Artists;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.Controllers;

[Route("artists")]
public class ArtistsController : Controller
{
    private readonly IArtistsManager _manager;

    public ArtistsController(IArtistsManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    [Route("")]
    public Task<IList<Artist>> GetAll() => _manager.GetAll();
    
    [HttpPut]
    [Route("")]
    public Task Create([FromBody] CreateArtistRequest request) => _manager.FindOrCreate(request);

    [HttpDelete]
    [Route("{userId}/{artistId}")]
    public Task Delete(string userId, string artistId) => _manager.Delete(userId, artistId);
}