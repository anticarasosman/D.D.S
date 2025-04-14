namespace Shin_Megami_Tensei;
using Shin_Megami_Tensei.data;

public class Abilities
{
    void Main(string[] args)
    {
        SkillManager skillManager = new SkillManager();
        List<Skills> skills = skillManager.LoadSkills(Path.Combine("data", "skills.json"));
    }
}