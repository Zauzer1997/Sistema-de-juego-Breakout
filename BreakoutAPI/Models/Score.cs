namespace BreakoutAPI.Models;

public class Score
{
    public int Id { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public int ScoreValue { get; set; }

    public DateTime DatePlayed { get; set; }
}