using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityVFX vfx;
    public float damage = 10f;

    [Header("Target detection")]
    [SerializeField]
    private Transform targetCheck;

    [SerializeField]
    private float targetCheckRadius = 1f;

    [SerializeField]
    private LayerMask whatIsTarget;

    public void Awake()
    {
        if (vfx == null)
        {
            vfx = GetComponent<EntityVFX>();
        }
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;
            damagable.TakeDamage(damage, transform);
            vfx.CreateHitVFX(target.transform);
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
