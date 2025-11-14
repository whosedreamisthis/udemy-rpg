using System.Collections;
using Unity.Mathematics;
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

    [Header("Electrify Effect Details")]
    [SerializeField]
    private GameObject lightningStrikeVFX;

    [SerializeField]
    private float currentCharge;

    [SerializeField]
    private float maximumCharge = 1;
    private Coroutine electrifiedCoroutine;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        entityHealth = GetComponent<EntityHealth>();
    }

    public void ApplyElectrifiedEffect(float duration, float damage, float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge += finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifiedEffect();
            return;
        }

        if (electrifiedCoroutine != null)
        {
            StopCoroutine(electrifiedCoroutine);
        }

        electrifiedCoroutine = StartCoroutine(ElectrifiedEffectCoroutine(duration));
    }

    private void StopElectrifiedEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVFX.StopAllVFX();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVFX, transform.position, quaternion.identity);
        entityHealth.ReduceHp(damage);
    }

    private IEnumerator ElectrifiedEffectCoroutine(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVFX.PlayerOnStatusVFX(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopElectrifiedEffect();
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
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
        {
            return true;
        }
        return currentEffect == ElementType.None;
    }
}
