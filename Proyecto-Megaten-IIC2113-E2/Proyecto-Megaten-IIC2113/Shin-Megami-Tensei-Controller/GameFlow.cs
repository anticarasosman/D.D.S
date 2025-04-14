using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class GameFlow
{
    private int round = 1;
    private Team teamP1, teamP2;
    private GameBoard gameBoard;
    private GameView gameView;
    private ActionView actionView;
    private TurnInitializer turnInitializer;
    public GameFlow(Team teamP1, Team teamP2, GameBoard gameBoard, GameView gameView, ActionView actionView)
    {
        this.teamP1 = teamP1;
        this.teamP2 = teamP2;
        this.gameBoard = gameBoard;
        this.gameView = gameView;
        this.actionView = actionView;
        this.turnInitializer = new TurnInitializer(gameView, actionView);
    }

    public void StartFlow()
    {
        SummonUnitsToBoard(teamP1, gameBoard.fieldJ1);
        SummonUnitsToBoard(teamP2, gameBoard.fieldJ2);
        Team currentPlayer = teamP1;
        Team opposingPlayer = teamP2;
        while (!teamP1.Defeated && !teamP2.Defeated)
        {
            currentPlayer = CheckCurrentPlayer(round);
            opposingPlayer = CheckOpposingPlayer(round);
            StartNewRound(currentPlayer, opposingPlayer);
            round++;
        }
        if (teamP1.Defeated){gameView.AnnounceWinner(teamP2);}
        else if (teamP2.Defeated){gameView.AnnounceWinner(teamP1);}
    }

    private void SummonUnitsToBoard(Team team, Unit[] playerBoard)
    {
        team.samurai.isSummoned = true;
        playerBoard[0] = team.samurai;
        int allowedMonstersOnBoard = 3;
        int currentMonstersOnBoard = 0;
        int allowedIteration = Math.Min(allowedMonstersOnBoard, team.monsters.Length);
        for (int i = 0; i < allowedIteration; i++)
        {
            team.monsters[i].isSummoned = true;
            playerBoard[i+1] = team.monsters[i];
        }
        currentMonstersOnBoard += team.monsters.Length;
        while (currentMonstersOnBoard < allowedMonstersOnBoard)
        {
            playerBoard[currentMonstersOnBoard] = new Unit();
            currentMonstersOnBoard++;
        }
    }
    private void StartNewRound(Team currentPlayer, Team opposingPlayer)
    {
        gameView.RoundAnnouncement(currentPlayer.samurai.name, currentPlayer.playerNumber);
        turnInitializer.InitalizeTurn(currentPlayer, opposingPlayer, gameBoard);
    }
    private Team CheckCurrentPlayer(int round)
    {
        if (round%2 == 1){return teamP1;}
        return teamP2;
    }
    private Team CheckOpposingPlayer(int round)
    {
        if (round%2 == 1){return teamP2;}
        return teamP1;
    }
}