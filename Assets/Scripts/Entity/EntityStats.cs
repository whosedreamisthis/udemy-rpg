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
        return (baseHp + bonusHp);
    }
}
