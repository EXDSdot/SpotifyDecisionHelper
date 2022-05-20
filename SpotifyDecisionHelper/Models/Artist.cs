using System.ComponentModel.DataAnnotations;
namespace SpotifyDecisionHelper.Models
{
    public class Artist
    {
        [Key] public int ID;
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Track> Tracks { get; }

        [Required] public List<int> Track_Ids;

        private static int curr_id;

        public Artist(string name, Track track)
        {
            Name = name;
            this.ID = curr_id++;
            Tracks.Add(track);
        }

        public Artist(string name)
        {
            this.ID = curr_id++;
            Name = name;
        }
        
        public Artist(string name, List<Track> tracks)
        {
            foreach (var track in tracks)
            {
                Tracks.Add(track);
            }
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
            Track_Ids.Add(track.Id);
        }
    }
}