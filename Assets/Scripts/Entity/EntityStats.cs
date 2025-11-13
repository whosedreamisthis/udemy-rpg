using System;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatsMajorGroup major;
    public StatsDefenseGroup defense;
    public StatsOffenseGroup offense;

    public float GetMaxHealth()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        return baseHp + bonusHp;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;
        float evasionCap = 75f;
        float totalEvasion = baseEvasion + bonusEvasion;
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);
        return finalEvasion;
    }
}
