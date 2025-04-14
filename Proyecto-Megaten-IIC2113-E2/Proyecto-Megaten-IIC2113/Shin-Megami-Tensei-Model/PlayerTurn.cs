namespace Shin_Megami_Tensei;

public class PlayerTurn
{
    public Team team;
    public int fullTurns;
    public int blinkingTurns;
    public bool GaveUp;
    public PlayerTurn(Team team, int fullTurns, int blinkingTurns)
    {
        this.team = team;
        this.fullTurns = fullTurns;
        this.blinkingTurns = blinkingTurns;
        this.GaveUp = false;
    }
}