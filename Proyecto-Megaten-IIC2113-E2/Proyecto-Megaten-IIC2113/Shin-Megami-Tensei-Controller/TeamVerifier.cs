using Shin_Megami_Tensei_Model;

namespace Shin_Megami_Tensei;

public class TeamVerifier
{
    public void VerifyTeam(Team team)
    {
        int maxAmountOfMonsters = 7;
        int maxAmountOfSkills = 8;
        if (team.samurai.name == "") {throw new InvalidTeamException("NO HAY SAMURAI");}
        if (team.monsters.Length > maxAmountOfMonsters) {throw new InvalidTeamException("HAY DEMASIADOS MONSTRUOS");}
        if (team.samurai.skillsInfo.Length > maxAmountOfSkills) {throw new InvalidTeamException("EL SAMURAI TIENE DEMASIADAS SKILLS");}
    }
}