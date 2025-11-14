using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityVFX vfx;
    private EntityStats stats;

    [Header("Target detection")]
    [SerializeField]
    private Transform targetCheck;

    [SerializeField]
    private float targetCheckRadius = 1f;

    [SerializeField]
    private LayerMask whatIsTarget;

    [Header("Status Effect Details")]
    [SerializeField]
    private float defaultDuration = 3;

    [SerializeField]
    private float chilledSlowMultiplier = 0.8f;

    public void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            float elementalDamage = stats.GetElementalDamage(out ElementType element);
            float damage = stats.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
            {
                ApplyStatusEffect(target.transform, element);
            }
            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateHitVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

        if (statusHandler == null)
        {
            return;
        }

        if (element == ElementType.Ice && statusHandler.CanBeApplied(element))
        {
            statusHandler.ApplyChilledEffect(defaultDuration, chilledSlowMultiplier * scaleFactor);
        }

        if (element == ElementType.Fire && statusHandler.CanBeApplied(element))
        {
            float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnedEffect(defaultDuration, fireDamage);
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
