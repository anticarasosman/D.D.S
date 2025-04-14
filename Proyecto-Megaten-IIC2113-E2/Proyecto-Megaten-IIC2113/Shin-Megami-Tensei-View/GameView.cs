using Shin_Megami_Tensei;

namespace Shin_Megami_Tensei_View;

public class GameView
{
    View gameView;
    public GameView(View view)
    {
        gameView = view;
    }
    public void RoundAnnouncement(string name, int playerNumber)
    {
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine($"Ronda de {name} (J{playerNumber})");
    }
    public void ShowGameState(GameBoard gameBoard)
    {
        Team player1 = gameBoard.teamJ1;
        Team player2 = gameBoard.teamJ2;
        gameView.WriteLine("----------------------------------------"); 
        ShowFieldState(player1);
        ShowFieldState(player2);
    }
    private void ShowFieldState(Team player)
    {
        string[] letter = ["B", "C", "D"];
        gameView.WriteLine($"Equipo de {player.samurai.name} (J{player.playerNumber})");
        gameView.WriteLine($"A-{CreateUnitStateMessage(player.samurai)}");
        for (int i = 0; i < letter.Length; i++)
        {
            if (i < player.monsters.Length)
            {
                if (player.monsters[i].stats.CurrentHp > 0){gameView.WriteLine($"{letter[i]}-{CreateUnitStateMessage(player.monsters[i])}");}
                else{gameView.WriteLine($"{letter[i]}-");}
            }
            else{gameView.WriteLine($"{letter[i]}-");}
        }
    }
    private string CreateUnitStateMessage(Unit unit)
    {
        return $"{unit.name} HP:{unit.stats.CurrentHp}/{unit.stats.HP} MP:{unit.stats.CurrentMp}/{unit.stats.MP}";
    }
    public void ShowCurrentTurns(int fullTurns, int blinkingTurns)
    {
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine($"Full Turns: {fullTurns}");
        gameView.WriteLine($"Blinking Turns: {blinkingTurns}");
    }
    public void ShowCurrentOrder(List<Unit> units, int iterations)
    {
        int boardPosition = 1;
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine("Orden:");
        for (int i = 0; i < units.Count; i++)
        {
            int currentUnit = (i + iterations) % units.Count;
            gameView.WriteLine($"{boardPosition}-{units[currentUnit].name}");
            boardPosition++;
        }
    }
    public int ChooseSamuraiActions(Unit samurai)
    {
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine($"Seleccione una acción para {samurai.name}");
        gameView.WriteLine("1: Atacar\n2: Disparar\n3: Usar Habilidad\n4: Invocar\n5: Pasar Turno\n6: Rendirse");
        int chosenAction = int.Parse(gameView.ReadLine());
        return chosenAction;
    }
    public int ChooseMonsterActions(Unit monster)
    {
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine($"Seleccione una acción para {monster.name}");
        gameView.WriteLine("1: Atacar\n2: Usar Habilidad\n3: Invocar\n4: Pasar Turno");
        int chosenAction = int.Parse(gameView.ReadLine());
        return chosenAction;
    }

    public void AnnounceWinner(Team team)
    {
        gameView.WriteLine("----------------------------------------");
        gameView.WriteLine($"Ganador: {team.samurai.name} (J{team.playerNumber})");
    }
}