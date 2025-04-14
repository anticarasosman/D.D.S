using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class TurnsManager
{
    public PlayerTurn turn;
    public ActionView actionView;
    public TurnsManager(PlayerTurn turn, ActionView actionView)
    {
        this.turn = turn;
        this.actionView = actionView;
    }
    public void ConsumeTurns(string affinity)
    {
        int fullTurnsChange = 0;
        int blinkingTurnsChange = 0;
        if (affinity == "-" || affinity == "Rs")
        {
            if (turn.blinkingTurns <= 0)
            {
                blinkingTurnsChange = -1;
                actionView.TurnsConsumedMessage(0, 1, 0);
            }
            else
            {
                fullTurnsChange = -1;
                actionView.TurnsConsumedMessage(1, 0, 0);
            }
        }
        else if (affinity == "Wk")
        {
            fullTurnsChange = -1; blinkingTurnsChange = 1;
            actionView.TurnsConsumedMessage(1, 0, 1);
        }
        else if (affinity == "Nu")
        {
            fullTurnsChange = -2; blinkingTurnsChange = -2;
            actionView.TurnsConsumedMessage(2, 2, 0);
        }
        //ESTO ES SOLO PARA TESTEAR LA E1
        else if (affinity == "Rp" || affinity == "Dr")
        {
            fullTurnsChange = -turn.fullTurns; blinkingTurnsChange = -turn.blinkingTurns;
            actionView.TurnsConsumedMessage(turn.fullTurns, turn.blinkingTurns, 0);
        }
        turn.fullTurns += fullTurnsChange;
        turn.blinkingTurns += blinkingTurnsChange;
    }
    public void PassTurn()
    {
        actionView.TurnsConsumedMessage(1, 0, 0);
    }
}