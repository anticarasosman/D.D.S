using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.data;

namespace Shin_Megami_Tensei.Acciones
{
    public class AccionFactory
    {
        public static Action CrearAccion(UserOptions opcion, Unit actingUnit, Game.Equipo jugadorActual, Game.Equipo oponente, View view, SkillManager skillManager)
        {
            Dictionary<UserOptions, Action> acciones = new Dictionary<UserOptions, Action>
            {
                { UserOptions.Attack, new Atacar(view, actingUnit, jugadorActual, oponente) },
                { UserOptions.Shoot, new Disparar(view, actingUnit, jugadorActual, oponente) },
                { UserOptions.UseSkill, new UsarHabilidad(view, actingUnit, jugadorActual, oponente, skillManager) },
                { UserOptions.Summon, new UsarHabilidad(view, actingUnit, jugadorActual, oponente) },
                { UserOptions.PassTurn, new UsarHabilidad(view, actingUnit, jugadorActual, oponente) },
                { UserOptions.GiveUp, new Rendirse(view, actingUnit, jugadorActual, oponente) }
            };
            return acciones.ContainsKey(opcion) ? acciones[opcion] : null;
        }
    }   
}