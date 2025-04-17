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
            if (turn.BlinkingTurns > 0)
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
            blinkingTurnsChange = 1;
            if (turn.FullTurns > 0)
            {
                fullTurnsChange = -1;
                actionView.TurnsConsumedMessage(1, 0, 1);
            }
            else
            {
                blinkingTurnsChange = -1;
                actionView.TurnsConsumedMessage(0, 1, 0);
            }
        }
        else if (affinity == "Nu")
        {
            Console.WriteLine("ENTRAMOS EN ESTA CONDICION");
            if (turn.BlinkingTurns >= 2)
            {
                blinkingTurnsChange = -2;
                actionView.TurnsConsumedMessage(0, 2, 0);
            }
            else if (turn.BlinkingTurns == 1)
            {
                fullTurnsChange = -1;
                blinkingTurnsChange = -1;
                actionView.TurnsConsumedMessage(1, 1, 0);
            }
            else if (turn.FullTurns == 1)
            {
                fullTurnsChange = -1;
                actionView.TurnsConsumedMessage(1, 0, 0);
            }
            else
            {
                fullTurnsChange = -2;
                actionView.TurnsConsumedMessage(2, 0, 0);
            }
        }
        else if (affinity == "Rp" || affinity == "Dr")
        {
            fullTurnsChange = -turn.FullTurns; blinkingTurnsChange = -turn.BlinkingTurns;
            actionView.TurnsConsumedMessage(turn.FullTurns, turn.BlinkingTurns, 0);
        }
        turn.FullTurns += fullTurnsChange;
        turn.BlinkingTurns += blinkingTurnsChange;
    }
    public void PassTurn()
    {
        int fullTurnsChange = 0;
        int blinkingTurnsChange = 0;
        if (turn.BlinkingTurns > 0)
        {
            blinkingTurnsChange = -1;
            actionView.TurnsConsumedMessage(0, 1, 0);
        }
        else
        {
            fullTurnsChange = -1;
            blinkingTurnsChange = 1;
            actionView.TurnsConsumedMessage(1, 0, 1);
        }
        turn.FullTurns += fullTurnsChange;
        turn.BlinkingTurns += blinkingTurnsChange;
    }
}