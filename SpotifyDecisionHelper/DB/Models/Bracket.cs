#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Bracket
{
    public string UserId { get; init; }
    public int BracketId { get; init; }
    
    public ICollection<Match>? Matches { get; set; }
}