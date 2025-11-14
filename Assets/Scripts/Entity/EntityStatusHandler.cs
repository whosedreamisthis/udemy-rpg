using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity;
    private EntityVFX entityVFX;
    private EntityStats entityStats;
    private EntityHealth entityHealth;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        entityHealth = GetComponent<EntityHealth>();
    }

    public void ApplyBurnedEffect(float duration, float totalDamage)
    {
        float fireRes = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDamage = totalDamage * (1 - fireRes);
        StartCoroutine(BurnedEffectCoroutine(duration, finalDamage));
    }

    private IEnumerator BurnedEffectCoroutine(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVFX.PlayerOnStatusVFX(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceRes = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceRes);
        StartCoroutine(ChilledEffectCoroutine(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCoroutine(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);

        currentEffect = ElementType.Ice;
        entityVFX.PlayerOnStatusVFX(duration, currentEffect);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
