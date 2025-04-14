using Shin_Megami_Tensei.data;
using System.Text.Json;
using Shin_Megami_Tensei_Model;

namespace Shin_Megami_Tensei;

public class Skill_Factory
{
    public Skills[] CreateSkills(string[] writtenSkills)
    {
        CheckRepeatedSkills(writtenSkills);
        Skills[] skills = new Skills[writtenSkills.Length];
        for (int i = 0; i < writtenSkills.Length; i++) { skills[i] = InstantiateSkills(writtenSkills[i]); }
        return skills;
    }
    public Skills? InstantiateSkills(string skillName)
    {
        string skillsPath = Path.Combine("data", "skills.json");
        string skillsJson = File.ReadAllText(skillsPath);
        List<Skills> allSkills = JsonSerializer.Deserialize<List<Skills>>(skillsJson);
        return allSkills.FirstOrDefault(skill => skill.name == skillName);
    }
    private void CheckRepeatedSkills(string[] writtenSkills)
    {
        if (writtenSkills.Length != writtenSkills.Distinct().Count()) {throw new InvalidTeamException("HABILIDADES REPETIDAS");}
    }
}