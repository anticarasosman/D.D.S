using Shin_Megami_Tensei_Model;
using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class ActionFactory
{
    public static Action CreateAction(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, UserOptions option, PlayerTurn turn)
    {
        Dictionary<UserOptions, Action> actions = new Dictionary<UserOptions, Action>
        {
            { UserOptions.Attack, new Attack(actingUnit, possibleTargets, actionView, turn) },
            { UserOptions.Shoot, new Shoot(actingUnit, possibleTargets, actionView, turn) },
            { UserOptions.UseSkill, new UseSkill(actingUnit, possibleTargets, actionView, turn) },
            { UserOptions.Summon, new Summon(actingUnit, possibleTargets, actionView, turn) },
            { UserOptions.PassTurn, new PassTurn(actingUnit, possibleTargets, actionView, turn) },
            { UserOptions.GiveUp, new GiveUp(actingUnit, possibleTargets, actionView, turn) }
        };
        return actions.ContainsKey(option) ? actions[option] : null;
    }
}