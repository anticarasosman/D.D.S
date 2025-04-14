using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.data;

namespace Shin_Megami_Tensei
{
    public abstract class Action
    {
         protected Unit ActingUnit;
         protected List<Unit> PossibleTargets;
         protected ActionView ActionView;
         protected AffinityChecker AffinityChecker;
         protected TurnsManager TurnsManager;
         protected PlayerTurn Turn;
         public Action(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
         {
             ActingUnit = actingUnit;
             PossibleTargets = possibleTargets;
             ActionView = actionView;
             AffinityChecker = new AffinityChecker();
             TurnsManager = new TurnsManager(turn, actionView);
         }
        public abstract bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn);
        public int Damage(Unit actingUnit, Unit target, string targetsAffinity, double damage)
        {
            double damageMultiplier = AffinityChecker.CheckDamageMultiplier(targetsAffinity);
            Console.WriteLine($"EL MULTIPLICADOR DE DAÑO ES {damageMultiplier}");
            int finalDamage = (int)(damageMultiplier * damage);
            if (targetsAffinity == "Repel"){actingUnit.stats.CurrentHp -= finalDamage;}
            else{target.stats.CurrentHp -= finalDamage;}
            if (target.stats.CurrentHp < 0){target.stats.CurrentHp = 0; target.isSummoned = false;}
            return finalDamage;
        }
    }
    public class Attack : Action
    {
        public Attack(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            int targetPosition = actionView.ChooseTarget(actingUnit, possibleTargets)-1;
            if (targetPosition >= possibleTargets.Count){return false;}
            string targetsAffinity = AffinityChecker.CheckAffinity("Phys", possibleTargets[targetPosition]);
            int modifier = (int)Math.Floor(actingUnit.stats.Str * 54 * 0.0114);
            int damage = Damage(actingUnit, possibleTargets[targetPosition], targetsAffinity, modifier);
            ActionView.AttackEffectMessage(actingUnit, possibleTargets[targetPosition], targetsAffinity, damage);
            TurnsManager.ConsumeTurns(targetsAffinity);
            return true;
        }
    }
    public class Shoot : Action
    {
        public Shoot(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            int targetPosition = actionView.ChooseTarget(actingUnit, possibleTargets)-1;
            if (targetPosition >= possibleTargets.Count){return false;}
            string targetsAffinity = AffinityChecker.CheckAffinity("Skl", possibleTargets[targetPosition]);
            int modifier = (int)Math.Floor(actingUnit.stats.Skl * 80 * 0.0114);
            int damage = Damage(actingUnit, possibleTargets[targetPosition], targetsAffinity, modifier);
            ActionView.ShootEffectMessage(actingUnit, possibleTargets[targetPosition], targetsAffinity, damage);
            TurnsManager.ConsumeTurns(targetsAffinity);
            return true;
        }
    }
    public class UseSkill : Action
    {
        //AHORA MISMO ABRIA QUE ROMPER EL PRINCIPIO DE APERTURA EN EL CASO DE NECESITAR MAS HABILIDADES DE CURACION, HABRA QUE CAMBIARLO PARA LA E3
        private SkillManager _skillManager;
        public UseSkill(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            int chosenSkill = ActionView.ChooseSkill(actingUnit.skillsInfo, actingUnit)-1;
            if (chosenSkill >= actingUnit.skillsInfo.Length){return false;}
            Skills skill = actingUnit.skillsInfo[chosenSkill];
            if (skill.type == "Heal"){DetermineHealingEffect(actingUnit, turn, skill, actionView);}
            else
            {
                int targetPosition = actionView.ChooseTarget(actingUnit, possibleTargets)-1;
                if (targetPosition > possibleTargets.Count){return false;}
                string targetsAffinity = AffinityChecker.CheckAffinity(skill.type, possibleTargets[targetPosition]);
                //ACAAAAA, NOS QUEDAMOS ACAAAAAA, HAY QUE PREGUNTARLE AL PROFE COMO CALCULAR BIEN EL DAÑO
                double modifier = Math.Sqrt(actingUnit.stats.Mag * skill.power);
                Console.WriteLine($"VAMOS A INFLINGIR {modifier} DE DAÑO");
                int damage = Damage(actingUnit, possibleTargets[targetPosition], targetsAffinity, modifier);
                ActionView.SkillEffectMessage(actingUnit, possibleTargets[targetPosition], targetsAffinity, damage, skill);
                TurnsManager.ConsumeTurns(targetsAffinity);
                return true;
            }
            return true;
        }
        private void DetermineHealingEffect(Unit actingUnit, PlayerTurn turn, Skills skill, ActionView actionView)
        {
            bool revive = CheckIfRevives(skill);
            if (revive)
            {
                List<Unit> revivalOptions = SelectRevivalOptions(turn.team.samurai, turn.team.monsters);
                int revivalTarget = actionView.ChooseRevivalTarget(actingUnit, revivalOptions)-1;
                Revive(revivalOptions[revivalTarget], skill.name);
            }
            else
            {
                List<Unit> healingOptions = SelectHealingOptions(turn.team.samurai, turn.team.monsters);
                int healingTarget = actionView.ChooseHealingTarget(actingUnit, healingOptions)-1;
                Heal(healingOptions[healingTarget], actingUnit, actionView, skill.power);
            }
        }
        private bool CheckIfRevives(Skills skill)
        {
            if (skill.name == "Recarm" || skill.name == "Samarecarm" || skill.name == "Invitation"){return true;}
            return false;
        }
        private List<Unit> SelectRevivalOptions(Unit samurai, Unit[] monsters)
        {
            List<Unit> possibleTargets = new List<Unit>();
            if(samurai.stats.CurrentHp == 0){possibleTargets.Add(samurai);}
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].stats.CurrentHp == 0) { possibleTargets.Add(monsters[i]); }
            }
            return possibleTargets;
        }
        private List<Unit> SelectHealingOptions(Unit samurai, Unit[] monsters)
        {
            List<Unit> possibleTargets = new List<Unit>();
            possibleTargets.Add(samurai);
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].isSummoned) { possibleTargets.Add(monsters[i]); }
            }
            return possibleTargets;
        }
        private void Revive(Unit revivalTarget, string skillName)
        {
            if (skillName == "Revive"){revivalTarget.stats.CurrentHp = (int)Math.Floor(revivalTarget.stats.HP*0.5);}
            else if (skillName == "Samarecarm"){revivalTarget.stats.CurrentHp = revivalTarget.stats.HP;}
            else if (skillName == "Invitation")
            {
                //ACA VA EL CODIGO DE INVOCAR
                revivalTarget.stats.CurrentHp = revivalTarget.stats.HP;
            }
        }
        private void Heal(Unit healingTarget, Unit healer, ActionView actionView, int healingPower)
        {
            int heal = healingTarget.stats.HP*(int)Math.Floor(healingTarget.stats.HP*(healingPower*0.01));
            actionView.HealingMessage(healer, healingTarget, heal);
            healingTarget.stats.CurrentHp += heal;
        }
    }
    public class Summon : Action
    {
        public Summon(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            Console.WriteLine("AHHHHHHHHHHH VOY A INVOCAR");
            return true;
        }
    }
    public class PassTurn : Action
    {
        public PassTurn(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            TurnsManager.PassTurn();
            return true;
        }
    }
    public class GiveUp : Action
    {
        public GiveUp(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn) : base(actingUnit, possibleTargets, actionView, turn) { }
        public override bool Execute(Unit actingUnit, List<Unit> possibleTargets, ActionView actionView, PlayerTurn turn)
        {
            actionView.GiveUpMessage(actingUnit.name, turn.team.playerNumber);
            turn.GaveUp = true;
            return true;
        }
    }
}