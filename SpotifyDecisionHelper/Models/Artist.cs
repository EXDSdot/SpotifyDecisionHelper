namespace SpotifyDecisionHelper.Models
{
    public class Artist
    {
        public string Name { get; set; }

        public List<Track> Tracks { get; }

        public static Artist operator +(Artist a1, Artist a2)
        {
            return a2;
        }

        public Artist(string name, Track track)
        {
            Name = name;
            Tracks.Add(track);
        }

        public Artist(string name)
        {
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
        }
    }
}