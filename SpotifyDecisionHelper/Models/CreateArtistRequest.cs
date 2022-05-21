namespace SpotifyDecisionHelper.Models;

public class CreateArtistRequest
{
    public string UserId { get; set; }
    public string ArtistId { get; set; }
        
    public string Name { get; set; }
    public int Rating { get; set; }
}