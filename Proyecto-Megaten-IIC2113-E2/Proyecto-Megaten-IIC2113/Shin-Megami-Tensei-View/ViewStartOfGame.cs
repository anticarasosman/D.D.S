namespace Shin_Megami_Tensei_View;

public class ViewStartOfGame
{
    View viewStartOfGame;
    
    public ViewStartOfGame(View view)
    {
        viewStartOfGame = view;
    }
    public void ShowFiles(string _teamsFolder)
    {
        viewStartOfGame.WriteLine("Elige un archivo para cargar los equipos");
        string[] teamsInText = Directory.GetFiles(_teamsFolder);
        ChooseFiles(teamsInText);
    }
    private void ChooseFiles(string[] teamsInText)
    {
        for (int i = 0; i < teamsInText.Length; i++)
        {
            int length = teamsInText[i].Length;
            viewStartOfGame.WriteLine($"{i}:"+" "+teamsInText[i][(length-7)..length]);
        }
    }
    public int GetSelectedTeams()
    {
        int chosenTeam = Convert.ToInt32(viewStartOfGame.ReadLine());
        return chosenTeam;
    }
}