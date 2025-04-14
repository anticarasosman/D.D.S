namespace Shin_Megami_Tensei;
public class Affinity
{
    public string Phys { get; set; }
    public string Gun { get; set; }
    public string Fire { get; set; }
    public string Ice { get; set; }
    public string Elec { get; set; }
    public string Force { get; set; }
    public string Light { get; set; }
    public string Dark { get; set; }
}

public class Unit
{
    public string name { get; set; }
    public Stats stats { get; set; }
    public Affinity affinity { get; set; }
    public bool summoned { get; set; }
    public string[] Abilities { get; set; }
}

public class Stats
{
    public int HP { get; set; }
    public int MP { get; set; }
    public int Str { get; set; }
    public int Skl { get; set; }
    public int Mag { get; set; }
    public int Spd { get; set; }
    public int Lck { get; set; }
    public int HpActual { get; set; }
}