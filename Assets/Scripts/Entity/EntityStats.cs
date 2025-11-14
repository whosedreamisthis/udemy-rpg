using System;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatsMajorGroup major;
    public StatsDefenseGroup defense;
    public StatsOffenseGroup offense;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * 2;
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;
        isCrit = UnityEngine.Random.Range(0f, 100f) <= critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
        return finalDamage;
    }

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
