namespace Shin_Megami_Tensei;

public class GameBoard
{
    private int lengthOfBoard = 4;
    public Team teamJ1;
    public Team teamJ2;
    public Unit[] fieldJ1;
    public Unit[] fieldJ2;

    public GameBoard(Team teamP1, Team teamP2)
    {
        teamJ1 = teamP1;
        teamJ2 = teamP2;
        fieldJ1 = new Unit[lengthOfBoard];
        fieldJ2 = new Unit[lengthOfBoard];
    }
}