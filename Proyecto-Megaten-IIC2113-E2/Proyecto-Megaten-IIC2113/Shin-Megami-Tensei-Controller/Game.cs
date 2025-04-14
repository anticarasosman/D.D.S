using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class Game
{
    private View _view;
    private string _teamsFolder;
    private TeamVerifier _teamVerifier;
    private ViewFactory _viewFactory;
    private TeamInitializer _teamInitializer;
    private GameInitializer _gameInitializer;
    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _viewFactory = new ViewFactory(_view);
        _teamInitializer = new TeamInitializer();
        _gameInitializer = new GameInitializer(_viewFactory);
        _teamVerifier = new TeamVerifier();
    }
    
    public void Play()
    {
        try
        {
            TryToPlay();
        }
        catch (Exception InvalidTeamException)
        {
            //ESTO ES SOLO PARA DEBUGGEAR
            _view.WriteLine("Error: "+ InvalidTeamException.Message);
            ViewInvalidTeams viewInvalidTeams = new ViewInvalidTeams(_view);
            viewInvalidTeams.ShowInvalidTeamMessage();
        }
    }

    private void TryToPlay()
    {
        ViewStartOfGame viewStartOfGame = _viewFactory.CreateViewStartOfGame();
        viewStartOfGame.ShowFiles(_teamsFolder);
        string[] teamsInText = Directory.GetFiles(_teamsFolder);
        int selectedTeam = viewStartOfGame.GetSelectedTeams();
        Team teamP1 = _teamInitializer.InitializeTeams(teamsInText, 1, selectedTeam);
        Team teamP2 = _teamInitializer.InitializeTeams(teamsInText, 2, selectedTeam);
        _teamVerifier.VerifyTeam(teamP1);
        _teamVerifier.VerifyTeam(teamP2);
        _gameInitializer.InitializeGame(teamP1, teamP2);
    }
}
