namespace Shin_Megami_Tensei;

public class TeamFactory
{
    public Unit samurai = new Unit();
    public Unit[] monsters = [];
    public Team BuildTeam(string[] teamInText, int playerNumber)
    {
        InstanciateUnits(teamInText);
        return new Team(samurai, monsters, playerNumber);
    }
    public void InstanciateUnits(string[] teamInText)
    {
        UnitFactory unitFactory = new UnitFactory();
        samurai = unitFactory.CreateSamurais(teamInText);
        monsters = unitFactory.CreateMonsters(teamInText);
    }

}