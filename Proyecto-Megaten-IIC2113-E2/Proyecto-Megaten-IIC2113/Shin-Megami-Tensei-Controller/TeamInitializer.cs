using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class TeamInitializer
{
    TeamFactory _teamFactory = new TeamFactory();
    public Team InitializeTeams(string[] teamsInText, int playerNumber, int chosenTeam)
    {
        string chosenTeamInText = teamsInText[chosenTeam];
        string[] teamMembersInText = SeparateTeams(chosenTeamInText, playerNumber);
        Team team = _teamFactory.BuildTeam(teamMembersInText, playerNumber);
        return team;
    }
    private string[] SeparateTeams(string teamList, int playerNumber)
    {
        string[] teamMembers = File.ReadAllLines(teamList);
        int i = 0;
        while (teamMembers[i] != "Player 2 Team") {i++;}
        if (playerNumber == 1) { return teamMembers[1..i]; } 
        return teamMembers[(i+1)..];
    }
}