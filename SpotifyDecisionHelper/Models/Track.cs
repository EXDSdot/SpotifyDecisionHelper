using System.ComponentModel.DataAnnotations;

namespace SpotifyDecisionHelper.Models
{

    public class Track
    {
        [Key] public string Name { set; get; }
        [Required]
        public string ArtistName { set; get; }
        //public Album Album { set; get; }
        //public Artist Artist { set; get; }

        /*public Track(Artist artist, string name)
        {
            Name = name;
            Artist = artist;
        }*/

        /*public Track(string artistName, string name)
        {
            Artist_Name = artistName;
            Name = name;
        }*/

        public Track(string name)
        {
            Name = name;
            ArtistName = "Blank";
        }
    }
}