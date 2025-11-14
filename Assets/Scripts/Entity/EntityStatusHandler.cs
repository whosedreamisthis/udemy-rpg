using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity;
    private EntityVFX entityVFX;
    private EntityStats stats;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceRes = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceRes);
        StartCoroutine(ChilledEffectCoroutine(reducedDuration, slowMultiplier));
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
