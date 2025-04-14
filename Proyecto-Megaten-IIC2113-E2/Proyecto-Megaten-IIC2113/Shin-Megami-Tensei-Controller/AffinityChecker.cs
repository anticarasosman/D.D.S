namespace Shin_Megami_Tensei;

public class AffinityChecker
{
    public string CheckAffinity(string element, Unit target)
    {
        if (element == "Phys"){return target.affinity.Phys;}
        if (element == "Gun"){return target.affinity.Gun;}
        if (element == "Fire"){return target.affinity.Fire;}
        if (element == "Ice"){return target.affinity.Ice;}
        if (element == "Elec"){return target.affinity.Elec;}
        if (element == "Force"){return target.affinity.Force;}
        if (element == "Light"){return target.affinity.Light;}
        if (element == "Dark"){return target.affinity.Dark;}
        return "";
    }
    public double CheckDamageMultiplier(string affinity)
    {
        if (affinity == "-"){return 1;}
        if (affinity == "Wk"){return 1.5;}
        if (affinity == "Rs"){return 0.5;}
        if (affinity == "Nu"){return 0;}
        if (affinity == "Rp"){return 0;}
        if (affinity == "Dr"){return -1;}
        return 1;
    }
}