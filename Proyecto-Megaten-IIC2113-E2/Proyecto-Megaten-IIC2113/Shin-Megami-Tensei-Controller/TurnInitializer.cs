using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class TurnInitializer
{
    GameView gameView;
    ActionView actionView;
    public TurnInitializer(GameView gameView, ActionView actionView)
    {
        this.gameView = gameView;
        this.actionView = actionView;
    }
    public void InitalizeTurn(Team team, Team opposingTeam, GameBoard gameBoard)
    {
        int fullTurns = DetermineFullTurns(team);
        PlayerTurn playerTurn = new PlayerTurn(team, fullTurns, 0);
        TurnBehaviour turnBehaviour = new TurnBehaviour(playerTurn, opposingTeam, gameBoard, gameView, actionView);
        turnBehaviour.Run();
    }
    private int DetermineFullTurns(Team team)
    {
        int fullTurns = 0;
        if (team.samurai.stats.CurrentHp > 0){fullTurns++;}
        for (int i = 0; i < team.monsters.Length; i++)
        {
            if (team.monsters[i].isSummoned && team.monsters[i].stats.CurrentHp > 0){fullTurns++;}
        }
        return fullTurns;
    }
}