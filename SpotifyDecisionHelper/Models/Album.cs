namespace SpotifyDecisionHelper.Models
{
    public class Album
    {
        public string Name { set; get; }
        public Artist Artist { set; get; }
        public List<Track> Tracks { get; }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }

        public Album(Artist artist, Track track)
        {
            Artist = artist;
            Tracks.Add(track);
        }

        public Album(Artist artist, List<Track> tracks)
        {
            foreach (var track in tracks)
            {
                Tracks.Add(track);
            }
        }
    }
}