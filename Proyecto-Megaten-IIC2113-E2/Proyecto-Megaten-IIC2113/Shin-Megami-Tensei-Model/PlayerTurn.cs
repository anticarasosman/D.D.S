namespace Shin_Megami_Tensei;

public class PlayerTurn
{
    public Team team;
    public int _fullTurns;
    public int _blinkingTurns;
    public bool GaveUp;
    
    public PlayerTurn(Team team, int fullTurns, int blinkingTurns)
    {
        this.team = team;
        this.FullTurns = fullTurns;
        this.BlinkingTurns = blinkingTurns;
        this.GaveUp = false;
    }
    
    public int FullTurns
    {
        get => _fullTurns;
        set => _fullTurns = value <= 0 ? 0 : value;
    }
    
    public int BlinkingTurns
    {
        get => _blinkingTurns;
        set => _blinkingTurns = value <= 0 ? 0 : value;
    }
}