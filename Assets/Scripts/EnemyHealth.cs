using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        if (damageDealer.GetComponent<Player>() != null)
        {
            Debug.Log("Enemy took damage from Player, trying to enter battle state.");
            enemy.TryEnterBattleState(damageDealer);
        }
        base.TakeDamage(damage, damageDealer);
    }
}
