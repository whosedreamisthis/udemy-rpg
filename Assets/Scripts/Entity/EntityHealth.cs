using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private EntityVFX entityVFX;
    private Entity entity;

    private EntityStats stats;

    [SerializeField]
    protected float currentHp;

    [SerializeField]
    protected bool isDead;

    [Header("Damage knockback")]
    [SerializeField]
    private Vector2 knockbackForce = new Vector2(5f, 5f);

    [SerializeField]
    private Vector2 heavyKnockbackForce = new Vector2(7, 7);

    [SerializeField]
    private float knockbackDuration = 0.2f;

    [SerializeField]
    private float heavyKnockbackDuration = 0.5f;

    [Header("Heavy damage knockback")]
    [SerializeField]
    private float heavyDamageThreshold = 0.3f;

    protected virtual void Awake()
    {
        entityVFX = GetComponent<EntityVFX>();
        entity = GetComponent<Entity>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponent<EntityStats>();
        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    private bool AttackEvaded() => Random.Range(0f, 100f) < stats.GetEvasion();

    public virtual bool TakeDamage(float damage, float elementalDamage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            return false;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        damage -= damage * mitigation;

        Vector2 knockback = calculateKnockback(damage, damageDealer);
        float duration = calculateKnockbackDuration(damage);
        entity.RecieveKnockback(knockback, duration);
        entityVFX?.PlayOnDamageVFX();

        ReduceHp(damage);
        Debug.Log($"Elemental Damage taken: {elementalDamage}");
        return true;
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    private Vector2 calculateKnockback(float damage, Transform damageDealer)
    {
        float direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackForce : knockbackForce;
        knockback.x *= direction;

        return knockback;
    }

    private float calculateKnockbackDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        return (damage / stats.GetMaxHealth()) >= heavyDamageThreshold;
    }
}
