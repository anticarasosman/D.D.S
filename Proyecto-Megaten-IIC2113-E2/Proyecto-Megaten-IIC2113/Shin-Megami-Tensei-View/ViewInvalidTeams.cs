namespace Shin_Megami_Tensei_View;

public class ViewInvalidTeams
{
    View viewInvalidTeams;
    public ViewInvalidTeams(View view)
    {
        viewInvalidTeams = view;
    }
    public void ShowInvalidTeamMessage()
    {
        viewInvalidTeams.WriteLine("Archivo de equipos inválido");
    }
}