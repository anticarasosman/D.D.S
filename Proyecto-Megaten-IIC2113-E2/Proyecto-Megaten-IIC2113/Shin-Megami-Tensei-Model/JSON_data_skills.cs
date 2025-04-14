namespace Shin_Megami_Tensei.data;
using System.Text.Json;
public class Skills
{
    public string name { get; set; }
    public string type { get; set; }
    public int cost { get; set; }
    public int power { get; set; }
    public string target { get; set; }
    public string hits { get; set; }
    public string effect { get; set; }
}

public class SkillManager
{
    public List<Skills> LoadSkills(string filePath)
    {
        List<Skills> skillsList = new List<Skills>();
        string json = File.ReadAllText(filePath);
        skillsList = JsonSerializer.Deserialize<List<Skills>>(json);
        return skillsList;
    }
}