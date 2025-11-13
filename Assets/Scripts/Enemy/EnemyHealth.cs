using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool targetWasHit = base.TakeDamage(damage, damageDealer);

        if (!targetWasHit)
        {
            return false;
        }

        if (damageDealer.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattleState(damageDealer);
        }

        return true;
    }
}
