namespace SpotifyDecisionHelper.Models;

public class CreateAlbumRequest
{
    public string UserId { get; set; }
    public string AlbumId { get; set; }
        
    public string Name { get; set; }
    public int Rating { get; set; }
        
    public string ArtistId { get; set;}
}