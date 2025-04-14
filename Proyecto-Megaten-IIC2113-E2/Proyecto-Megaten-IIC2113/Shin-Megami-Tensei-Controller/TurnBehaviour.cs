using Shin_Megami_Tensei_Model;
using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class TurnBehaviour
{
    PlayerTurn turn;
    Team opposingTeam;
    GameView gameView;
    ActionView actionView;
    GameBoard gameBoard;
    public TurnBehaviour(PlayerTurn turn, Team opposingTeam, GameBoard gameBoard, GameView gameView, ActionView actionView)
    {
        this.turn = turn;
        this.opposingTeam = opposingTeam;
        this.gameBoard = gameBoard;
        this.gameView = gameView;
        this.actionView = actionView;
    }
    public void Run()
    {
        int iterations = 0;
        List<Unit> unitsBySpeed = OrderBySpd(turn.team);
        while (turn.fullTurns > 0)
        {
            List<Unit> activeUnits = ChooseActiveUnits(unitsBySpeed);
            List<Unit> possibleTargets = ChoosePossibleTargets(opposingTeam);
            gameView.ShowGameState(gameBoard);
            gameView.ShowCurrentTurns(turn.fullTurns, turn.blinkingTurns);
            gameView.ShowCurrentOrder(activeUnits, iterations);
            ChooseActions(activeUnits[iterations], possibleTargets, turn);
            iterations++;
            bool someoneWon = CheckForWinner(turn, opposingTeam);
            if (someoneWon) {break;}
        }
    }
    private List<Unit> OrderBySpd(Team team)
    {
        List<Unit> unitsOrdered = new List<Unit>();
        unitsOrdered.Add(team.samurai);
        for (int i = 0; i < team.monsters.Length; i++) { unitsOrdered.Add(team.monsters[i]); }
        return unitsOrdered.OrderByDescending(unit => unit.stats.Spd).ToList();
    }
    private List<Unit> ChooseActiveUnits(List<Unit> team)
    {
        List<Unit> activeUnits = new List<Unit>();
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].isSummoned && team[i].stats.CurrentHp > 0)
            {
                activeUnits.Add(team[i]);
            }
        }
        return activeUnits;
    }
    private List<Unit> ChoosePossibleTargets(Team team)
    {
        int maximunMonsterTargets = 3;
        int currentPossibleTargets = Math.Min(team.monsters.Length, maximunMonsterTargets);
        List<Unit> possibleTargets = new List<Unit>();
        if(team.samurai.stats.CurrentHp > 0){possibleTargets.Add(team.samurai);}
        for (int i = 0; i < currentPossibleTargets; i++)
        {
            if (team.monsters[i].isSummoned && team.monsters[i].stats.CurrentHp > 0) { possibleTargets.Add(team.monsters[i]); }
        }
        return possibleTargets;
    }
    private void ChooseActions(Unit unit, List<Unit> possibleTargets, PlayerTurn turn)
    {
        int userOption;
        if (unit.isSamurai){userOption = gameView.ChooseSamuraiActions(unit);}
        else{userOption = gameView.ChooseMonsterActions(unit);}
        UserOptions option = ParseUserInputIntoAction(userOption, unit.isSamurai);
        Action chosenAction = ActionFactory.CreateAction(unit, possibleTargets, actionView, option, turn);
        ExecuteAction(chosenAction, unit, possibleTargets, turn);
    }
    private void ExecuteAction(Action chosenAction, Unit unit, List<Unit> possibleTargets, PlayerTurn turn)
    {
        bool ActionExecuted = chosenAction.Execute(unit, possibleTargets, actionView, turn);
        if (!ActionExecuted) { ChooseActions(unit, possibleTargets, turn); }
    }
    public UserOptions ParseUserInputIntoAction(int userInput, bool isSamurai)
    {
        if (isSamurai)
        {
            if (userInput == 1) return UserOptions.Attack;
            if (userInput == 2) return UserOptions.Shoot;
            if (userInput == 3) return UserOptions.UseSkill;
            if (userInput == 4) return UserOptions.Summon;
            if (userInput == 5) return UserOptions.PassTurn;
            if (userInput == 6) return UserOptions.GiveUp;
        }
        else
        {
            if (userInput == 1) return UserOptions.Attack;
            if (userInput == 2) return UserOptions.UseSkill;
            if (userInput == 3) return UserOptions.Summon;
            if (userInput == 4) return UserOptions.PassTurn;
        }
        return UserOptions.GiveUp;
    }
    public bool CheckForWinner(PlayerTurn turn, Team opposingTeam)
    {
        if (turn.GaveUp) { turn.team.Defeated = true; return true; }
        if (opposingTeam.samurai.stats.CurrentHp > 0){return false;}
        for (int i = 0; opposingTeam.monsters.Length > i; i++)
        {
            if (opposingTeam.monsters[i].stats.CurrentHp > 0 && opposingTeam.monsters[i].isSummoned){return false;}
        }
        opposingTeam.Defeated = true;
        return true;
    }
}