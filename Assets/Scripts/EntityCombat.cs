using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public float damage = 10f;

    [Header("Target detection")]
    [SerializeField]
    private Transform targetCheck;

    [SerializeField]
    private float targetCheckRadius = 1f;

    [SerializeField]
    private LayerMask whatIsTarget;

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            EntityHealth targetHealth = target.GetComponent<EntityHealth>();
            targetHealth?.TakeDamage(damage, transform);
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
