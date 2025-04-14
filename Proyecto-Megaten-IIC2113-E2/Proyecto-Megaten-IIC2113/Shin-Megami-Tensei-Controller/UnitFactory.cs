using System.Text.RegularExpressions;
using System.Text.Json;
using Shin_Megami_Tensei_Model;

namespace Shin_Megami_Tensei;

public class UnitFactory
{
    Skill_Factory _skillFactory = new Skill_Factory();
    private string samuraiName = "";
    private string skills = "";
    public Unit CreateSamurais(string[] teamList)
    {
        Dictionary<string, string> namesAndSkills = PrepareSamurai(teamList);
        Unit samuraiUnit = InstanciateSamurai(namesAndSkills);
        return samuraiUnit;
    }
    Dictionary<string, string> PrepareSamurai(string[] teamList)
    {
        //Dejar los patrones en esta funcion y llamar al metodo de hacer Match dos veces con las distintas listas y colocamos el throw Exception. de si hay mas de un samurai o si no hay samurai.
        string samuraiNamePattern = @"(?<=\[Samurai\]\s)([^\s()]+)";
        string samuraiSkillPattern = @"\(([^)]+)\)";
        MatchSamurai(teamList, samuraiNamePattern, samuraiSkillPattern);
        Dictionary<string, string> results = new Dictionary<string, string>
        {
            { "nombreSamurai", samuraiName },
            { "habilidades", skills.Replace("(", "").Replace(")", "") },
        };
        return results;
    }

    private void MatchSamurai(string[] teamList, string samuraiNamePattern, string samuraiSkillPattern)
    {
        Match samuraiNameRegex = Regex.Match(teamList[0], samuraiNamePattern);
        for (int i = 1; i < teamList.Length; i++){CheckForMoreSamurais(teamList[i]);}
        Match samuraiSkillRegex = Regex.Match(teamList[0], samuraiSkillPattern);
        samuraiName = samuraiNameRegex.Value;
        skills = samuraiSkillRegex.Value;
    }
    private void CheckForMoreSamurais(string Unit)
    {
        if (Unit.Contains("Samurai")) { throw new InvalidTeamException("SAMURAIS REPETIDOS"); }
    }
    
    Unit InstanciateSamurai(Dictionary<string, string> nombresYHabilidades)
    {
        Unit samurai = new Unit();
        samurai.name = "";
        string myJsonSamurais = File.ReadAllText(Path.Combine("data", "samurai.json"));
        var samurais = JsonSerializer.Deserialize<List<Unit>>(myJsonSamurais);
        foreach (var s in samurais)
        {
            if (s.name == nombresYHabilidades["nombreSamurai"]) {samurai = s;}
        }
        if (samurai.name != "")
        {
            SetUnitState(samurai);
            samurai.isSamurai = true;
            samurai.skillsInfo = _skillFactory.CreateSkills(nombresYHabilidades["habilidades"].Split(","));
        }
        return samurai;
    }
    public Unit[] CreateMonsters(string[] listaEquipo)
    {
        Unit[] monsterList = new Unit[listaEquipo.Length-1];
        monsterList = FillUpMonsterList(monsterList);
        for (int i = 1; i < listaEquipo.Length; i++)
        {
            Unit monsterAdded = InstantiateMonsters(listaEquipo[i]);
            SetUnitState(monsterAdded);
            CheckForDuplicateMonster(monsterList, monsterAdded);
            monsterList[i - 1] = monsterAdded;
        }
        return monsterList;
    }
    private void CheckForDuplicateMonster(Unit[] monsterList, Unit monster)
    {
        for (int i = 0; i < monsterList.Length; i++)
        {
            if (monsterList[i].name == monster.name){throw new InvalidTeamException("HAY MONSTRUOS DUPLICADOS");}
        }
    }
    private Unit[] FillUpMonsterList(Unit[] monsterList)
    {
        for (int i = 0; i < monsterList.Length; i++) { monsterList[i] = new Unit(); }
        return monsterList;
    }
    private Unit InstantiateMonsters(string monsterName)
    {
        string myJsonMonsters = File.ReadAllText(Path.Combine("data", "monsters.json"));
        List<Unit> allMonsters = JsonSerializer.Deserialize<List<Unit>>(myJsonMonsters);
        //Ahora mismo no entiendo muy bien como funcionan las habilidades de monstruo, voy a asumir que ya vienen instanciadas y funcionara solo.
        //Si no funciona, intenta usar la funcion InstantiateSkills() de Skill_Factory para reemplazar las habilidades directamente.
        Unit monster = allMonsters.FirstOrDefault(monster => monster.name == monsterName);
        monster.skillsInfo = _skillFactory.CreateSkills(monster.skills);
        return monster;
    }
    private void SetUnitState(Unit unit)
    {
        unit.stats.CurrentHp = unit.stats.HP;
        unit.stats.CurrentMp = unit.stats.MP;
    }
}