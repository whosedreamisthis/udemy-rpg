using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    private EntityVFX entityVFX;

    [SerializeField]
    protected float maxHp = 100;

    [SerializeField]
    protected bool isDead;

    protected virtual void Awake()
    {
        entityVFX = GetComponent<EntityVFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        entityVFX?.PlayOnDamageVFX();

        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if (maxHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " has died.");
    }
}
