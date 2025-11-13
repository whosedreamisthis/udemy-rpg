using System;
using UnityEngine;

[Serializable]
public class StatsDefenseGroup
{
    //phsical defense
    public Stat armor;
    public Stat evasion;

    //elemental defense
    public Stat fireRes;
    public Stat iceRes;
    public Stat lightningRes;
}
