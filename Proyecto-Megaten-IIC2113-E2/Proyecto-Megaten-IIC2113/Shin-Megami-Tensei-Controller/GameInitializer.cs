using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class GameInitializer
{
    ViewFactory _viewFactory;
    public GameInitializer(ViewFactory viewFactory)
    {
        _viewFactory = viewFactory;
    }
    public void InitializeGame(Team teamP1, Team teamP2)
    {
        GameView gameView = _viewFactory.CreateGameView();
        ActionView actionView = _viewFactory.CreateActionView();
        GameBoard gameBoard = new GameBoard(teamP1, teamP2);
        GameFlow gameFlow = new GameFlow(teamP1, teamP2, gameBoard, gameView, actionView);
        gameFlow.StartFlow();
    }
}