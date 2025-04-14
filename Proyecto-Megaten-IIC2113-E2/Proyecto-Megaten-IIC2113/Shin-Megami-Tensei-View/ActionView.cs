using Shin_Megami_Tensei;
using Shin_Megami_Tensei.data;

namespace Shin_Megami_Tensei_View;

public class ActionView
{
    View actionView;
    public ActionView(View view)
    {
        actionView = view;
    }
    public int ChooseTarget(Unit attacker, List<Unit> possibleTargets)
    {
        int currentTarget = 0;
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine($"Seleccione un objetivo para {attacker.name}");
        for (int i = 0; i < possibleTargets.Count; i++)
        {
            if (possibleTargets[i].isSummoned && possibleTargets[i].stats.CurrentHp > 0)
            {
                actionView.WriteLine($"{currentTarget+1}-"+CreateUnitStateMessage(possibleTargets[i]));
                currentTarget++;
            }
        }
        actionView.WriteLine($"{currentTarget+1}-Cancelar");
        int chosenTarget = int.Parse(actionView.ReadLine());
        return chosenTarget;
    }
    private string CreateUnitStateMessage(Unit unit)
    {
        return $"{unit.name} HP:{unit.stats.CurrentHp}/{unit.stats.HP} MP:{unit.stats.CurrentMp}/{unit.stats.MP}";
    }
    public void AttackEffectMessage(Unit attacker, Unit target, string affinity, double damage)
    {
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine(DamageTypeMessage("Phys", attacker.name, target.name));
        actionView.WriteLine($"{target.name} recibe {damage} de daño");
        actionView.WriteLine(AffinityMessage(attacker, target, affinity, damage));
    }
    public void ShootEffectMessage(Unit attacker, Unit target, string affinity, double damage)
    {
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine(DamageTypeMessage("Gun", attacker.name, target.name));
        actionView.WriteLine($"{target.name} recibe {damage} de daño");
        actionView.WriteLine(AffinityMessage(attacker, target, affinity, damage));
    }
    private string AffinityMessage(Unit attacker, Unit target, string affinity, double damage)
    {
        if (affinity == "Wk"){return$"{target.name} es débil contra el ataque de {attacker.name}";}
        if (affinity == "Rs"){return$"{target.name} es resistente contra el ataque de {attacker.name}";}
        if (affinity == "Null"){return$"{target.name} bloquea el ataque de {attacker.name}";}
        if (affinity == "Rp"){return$"{target.name} devuelve {damage} daño a {attacker.name}";}
        if (affinity == "Dr"){return$"{target.name} absorbe {-damage} daño";}
        return$"{target.name} termina con HP:{target.stats.CurrentHp}/{target.stats.HP}";
    }
    public void TurnsConsumedMessage(int fullTurnsConsumed, int blinkingTurnsConsumed, int blinkingTurnsGained)
    {
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine($"Se han consumido {fullTurnsConsumed} Full Turn(s) y {blinkingTurnsConsumed} Blinking Turn(s)");
        actionView.WriteLine($"Se han obtenido {blinkingTurnsGained} Blinking Turn(s)");
    }
    public void GiveUpMessage(string playerName, int playerNumber)
    {
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine($"{playerName} (J{playerNumber}) se rinde");
    }
    public int ChooseSkill(Skills[] skillInfo, Unit unit)
    {
        int i = 0;
        int tooExpensiveSkills = 0;
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine($"Seleccione una habilidad para que {unit.name} use");
        int currentPosition = 1;
        if (skillInfo[0] != null)
        {
            for (i = 0; i < skillInfo.Length; i++)
            {
                if (skillInfo[i].cost <= unit.stats.CurrentMp)
                {
                    actionView.WriteLine($"{currentPosition}-"+skillInfo[i].name+$" MP:{skillInfo[i].cost}");
                    currentPosition++;
                }
                else{tooExpensiveSkills++;}
            }
        }
        actionView.WriteLine($"{currentPosition}-Cancelar");
        int chosenSkill = int.Parse(actionView.ReadLine());
        return chosenSkill+tooExpensiveSkills;
    }
    public void SkillEffectMessage(Unit attacker, Unit target, string affinity, int damage, Skills skill)
    {
        actionView.WriteLine("----------------------------------------");
        actionView.WriteLine(DamageTypeMessage(skill.type, attacker.name, target.name));
        actionView.WriteLine(AffinityMessage(attacker, target, affinity, damage));
        actionView.WriteLine($"{target.name} recibe {damage} de daño");
    }
    private string DamageTypeMessage(string skillElement, string attacker, string target)
    {
        if (skillElement == "Phys"){return $"{attacker} ataca a {target}";}
        if (skillElement == "Gun"){return $"{attacker} dispara a {target}";}
        if (skillElement == "Fire"){return $"{attacker} lanza fuego a {target}";}
        if (skillElement == "Ice"){return $"{attacker} lanza hielo a {target}";}
        if (skillElement == "Elect"){return $"{attacker} lanza electricidad a {target}";}
        return $"{attacker} lanza viento a {target}";
    }
    public int ChooseRevivalTarget(Unit unit, List<Unit> revivalTargets)
    {
        int i;
        actionView.WriteLine($"Seleccione un objetivo para {unit.name}");
        for (i = 0; i < revivalTargets.Count; i++)
        {
            actionView.WriteLine($"{i+1}-"+CreateUnitStateMessage(revivalTargets[i]));
        }
        actionView.WriteLine($"{i+1}-Cancelar");
        int chosenTarget = int.Parse(actionView.ReadLine());
        return chosenTarget;
    }
    public int ChooseHealingTarget(Unit unit, List<Unit> healingTargets)
    {
        int i;
        actionView.WriteLine($"Seleccione un objetivo para {unit.name}");
        for (i = 0; i < healingTargets.Count; i++)
        {
            actionView.WriteLine($"{i+1}-"+CreateUnitStateMessage(healingTargets[i]));
        }
        actionView.WriteLine($"{i+1}-Cancelar");
        int chosenTarget = int.Parse(actionView.ReadLine());
        return chosenTarget;
    }
    public void HealingMessage(Unit healer, Unit healingTarget, int heal)
    {
        actionView.WriteLine($"{healer.name} cura a {healingTarget.name}");
        actionView.WriteLine($"{healingTarget.name} recibe {heal} de HP");
        actionView.WriteLine($"{healingTarget.name} termina con HP:{healingTarget.stats.CurrentHp}/{healingTarget.stats.HP}");
    }
}