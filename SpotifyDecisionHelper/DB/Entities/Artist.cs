using System.ComponentModel.DataAnnotations;

namespace SpotifyDecisionHelper.DB.Entities
{
    public class Artist
    {
        public string UserId { get; set; }
        [Key]
        public string ArtistId { get; set; }
        
        public string Name { get; set; }
        public int Rating { get; set; }

        public ICollection<Album>? Albums { get; set; }
    }
}