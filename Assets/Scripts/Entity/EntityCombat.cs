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

    public void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;
            bool isCrit;
            bool targetGotHit = damagable.TakeDamage(
                stats.GetPhysicalDamage(out isCrit),
                transform
            );

            if (targetGotHit)
                vfx.CreateHitVFX(target.transform, isCrit);
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
