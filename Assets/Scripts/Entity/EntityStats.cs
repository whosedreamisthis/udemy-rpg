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

    public float GetArmorReduction()
    {
        float finalArmorReduction = offense.armorReduction.GetValue() / 100;
        return finalArmorReduction;
        // float baseArmorReduction = offense.armorReduction.GetValue();
        // float bonusArmorReduction = major.agility.GetValue() * 0.2f;
        // float totalArmorReduction = baseArmorReduction + bonusArmorReduction;
        // float armorReductionCap = 75f;
        // float finalArmorReduction = Mathf.Clamp(totalArmorReduction, 0, armorReductionCap);
        // return finalArmorReduction;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.85f;
        mitigation = Mathf.Clamp(mitigation, 0, mitigationCap);
        return mitigation;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;
        return finalMaxHealth;
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
