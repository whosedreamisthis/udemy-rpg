using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField]
    protected float maxHp = 100;

    [SerializeField]
    protected bool isDead;

    public virtual void TakeDamage(float damage)
    {
        if (isDead)
            return;

        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage.");

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
