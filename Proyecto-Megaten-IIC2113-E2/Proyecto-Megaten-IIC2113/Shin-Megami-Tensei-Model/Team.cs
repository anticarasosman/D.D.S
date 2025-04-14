namespace Shin_Megami_Tensei;

public class Team
{
    public Unit samurai;
    public Unit[] monsters;
    public int playerNumber;
    public bool Defeated;

    public Team(Unit samurai, Unit[] monsters, int playerNumber)
    {
        this.samurai = samurai;
        this.monsters = monsters;
        this.playerNumber = playerNumber;
        this.Defeated = false;
    }
}