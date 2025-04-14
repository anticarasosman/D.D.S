namespace Shin_Megami_Tensei;

public class Printer(Unit Unidad)
{
    public string PrintUnitStats(Unit Unidad)
    {
        return ($"HP: {Unidad.stats.HpActual}/{Unidad.stats.HP} MP:{Unidad.stats.MP}/{Unidad.stats.MP}");
    }
}