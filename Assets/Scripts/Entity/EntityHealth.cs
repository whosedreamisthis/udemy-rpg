using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private EntityVFX entityVFX;
    private Entity entity;

    private EntityStats entityStats;

    [SerializeField]
    protected float currentHealth;

    [SerializeField]
    protected bool isDead;

    [Header("Health Regen")]
    [SerializeField]
    private float regenInterval;

    [SerializeField]
    private bool canRegenerateHealth = true;

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
        entityStats = GetComponent<EntityStats>();
        currentHealth = entityStats.GetMaxHealth();
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    private bool AttackEvaded() => Random.Range(0f, 100f) < entityStats.GetEvasion();

    public virtual bool TakeDamage(
        float damage,
        float elementalDamage,
        ElementType element,
        Transform damageDealer
    )
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            return false;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        return true;
    }

    private void RegenerateHealth()
    {
        if (!canRegenerateHealth)
            return;
        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX();

        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
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

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }

    private Vector2 calculateKnockback(float damage, Transform damageDealer)
    {
        float direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackForce : knockbackForce;
        knockback.x *= direction;

        return knockback;
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = calculateKnockback(finalDamage, damageDealer);
        float duration = calculateKnockbackDuration(finalDamage);
        entity.RecieveKnockback(knockback, duration);
    }

    private float calculateKnockbackDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        return (damage / entityStats.GetMaxHealth()) >= heavyDamageThreshold;
    }
}
