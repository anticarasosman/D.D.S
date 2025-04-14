using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.data;

namespace Shin_Megami_Tensei.Acciones
{
    public abstract class Action
    {
         protected View View;
         protected Unit ActingUnit;
         protected Game.Equipo JugadorActual;
         protected Game.Equipo Oponente;
         public Action(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
         {
             View = view;
             ActingUnit = actingUnit;
             JugadorActual = jugadorActual;
             Oponente = oponente;
         }
        public abstract void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente);
    }
    
    public class Atacar : Action
    {
        public Atacar(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente) : base(view, actingUnit, jugadorActual, oponente) { }

        public override void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
        {
            int i;
            int suma = 1;
            View.WriteLine($"Seleccione un objetivo para {actingUnit.name}");
            if (oponente.samurai.stats.HpActual != 0)
            {
                View.WriteLine($"1-{oponente.samurai.name} HP:{oponente.samurai.stats.HpActual}/{oponente.samurai.stats.HP} MP:{oponente.samurai.stats.MP}/{oponente.samurai.stats.MP}");
                suma = 2;
            }
            for (i = 0; i < oponente.monstruos.Length; i++)
            {
                View.WriteLine($"{i + suma}-{oponente.monstruos[i].name} HP:{oponente.monstruos[i].stats.HpActual}/{oponente.monstruos[i].stats.HP} MP:{oponente.monstruos[i].stats.MP}/{oponente.monstruos[i].stats.MP}");
                if (i == 2) {i++; break;}
            }
            View.WriteLine($"{i + suma}-Cancelar");
            int opcion = int.Parse(View.ReadLine());
            if (opcion == 1) {Dañar(actingUnit, oponente.samurai);}
            else {Dañar(actingUnit, oponente.monstruos[opcion-suma]);}
        }
        public void Dañar(Unit actingUnit, Unit objetivo)
        {
            View.WriteLine("----------------------------------------");
            int daño = (int)Math.Floor(actingUnit.stats.Str * 0.6156);
            View.WriteLine($"{actingUnit.name} ataca a {objetivo.name}");
            View.WriteLine($"{objetivo.name} recibe {daño} de daño");
            objetivo.stats.HpActual -= daño;
            if (objetivo.stats.HpActual <= 0)
            {   
                objetivo.stats.HpActual=0; objetivo.summoned = false;
            }
            View.WriteLine($"{objetivo.name} termina con HP:{objetivo.stats.HpActual}/{objetivo.stats.HP}");
            View.WriteLine("----------------------------------------");
        }
    }
    
    public class Disparar : Action
    {
        public Disparar(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente) : base(view, actingUnit, jugadorActual, oponente) { }
        public override void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
        {
            int i;
            int suma = 1;
            View.WriteLine($"Seleccione un objetivo para {actingUnit.name}");
            if (oponente.samurai.stats.HpActual != 0)
            {
                View.WriteLine($"1-{oponente.samurai.name} HP:{oponente.samurai.stats.HpActual}/{oponente.samurai.stats.HP} MP:{oponente.samurai.stats.MP}/{oponente.samurai.stats.MP}");
                suma = 2;
            }
            for (i = 0; i < oponente.monstruos.Length; i++)
            {
                View.WriteLine($"{i + suma}-{oponente.monstruos[i].name} HP:{oponente.monstruos[i].stats.HpActual}/{oponente.monstruos[i].stats.HP} MP:{oponente.monstruos[i].stats.MP}/{oponente.monstruos[i].stats.MP}");
                if (i == 2) {i++; break;}
            }
            View.WriteLine($"{i + suma}-Cancelar");
            int opcion = int.Parse(View.ReadLine());
            if (opcion == i+suma) { jugadorActual.cancelar = true; }
            if (opcion == oponente.monstruos.Length + suma || opcion == 5) { return; }
            if (opcion == 1) {Dañar(actingUnit, oponente.samurai);}
            else {Dañar(actingUnit, oponente.monstruos[opcion-suma]);}
        }
        private void Dañar(Unit actingUnit, Unit objetivo)
        {
            View.WriteLine("----------------------------------------");
            int daño = (int)Math.Floor(actingUnit.stats.Skl * 0.912);
            View.WriteLine($"{actingUnit.name} dispara a {objetivo.name}");
            View.WriteLine($"{objetivo.name} recibe {daño} de daño");
            objetivo.stats.HpActual -= daño;
            if (objetivo.stats.HpActual <= 0) { objetivo.stats.HpActual=0; objetivo.summoned = false; }
            View.WriteLine($"{objetivo.name} termina con HP:{objetivo.stats.HpActual}/{objetivo.stats.HP}");
            View.WriteLine("----------------------------------------");
        }
    }
    
    public class UsarHabilidad : Action
    {
        private SkillManager _skillManager;
        public UsarHabilidad(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente, SkillManager skillManager)
            : base(view, actingUnit, jugadorActual, oponente)
        {
            _skillManager = skillManager;
        }
        public override void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
        {
            List<Skills> skillsList = _skillManager.LoadSkills(Path.Combine("data", "skills.json"));
            var habilidadesDisponibles = skillsList
                .Where(skill => actingUnit.Abilities.Contains(skill.name))
                .ToList();
            View.WriteLine($"Seleccione una habilidad para que {actingUnit.name} use");
            for (int i = 0; i < habilidadesDisponibles.Count; i++)
            {
                View.WriteLine($"{i + 1}-{habilidadesDisponibles[i].name} MP:{habilidadesDisponibles[i].cost}");
            }
            View.WriteLine($"{habilidadesDisponibles.Count + 1}-Cancelar");
            int opcion = int.Parse(View.ReadLine());
            if (opcion == habilidadesDisponibles.Count+1){jugadorActual.cancelar = true;}
        }
    }
    
    public class Invocar : Action
    {
        public Invocar(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente) : base(view, actingUnit, jugadorActual, oponente) { }
        public override void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
        {
            View.WriteLine("VOY A INVOCAR");
            throw new NotImplementedException();
        }
    }
    
    public class PasarTurno : Action
    {
        public PasarTurno(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente) : base(view, actingUnit, jugadorActual, oponente) { }
        public override void Ejecutar(Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente)
        {
            View.WriteLine("VOY A PASAR MI TURNO");
            throw new NotImplementedException();
        }
    }
    public class Rendirse : Action
    {
        public Rendirse(View view, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente) : base(view, actingUnit, jugadorActual, oponente) { }
        public override void Ejecutar(Unit usuario, Game.Equipo jugadorActual, Game.Equipo equipoEnemigo)
        {
            if (jugadorActual.nroJugador == 1) { View.WriteLine($"{jugadorActual.samurai.name} (J1) se rinde"); }
            else {View.WriteLine($"{jugadorActual.samurai.name} (J2) se rinde");}
            jugadorActual.meRindo = true;
        }
    }
}