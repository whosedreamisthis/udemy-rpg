using UnityEngine;

public class Enemy_AnimationTriggers : EntityAnimationTriggers
{
    private Enemy enemy;
    private EnemyVFX enemyVFX;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<EnemyVFX>();
    }

    private void EnableCounterWindow()
    {
        enemy.EnableCounterWindow(true);
        enemyVFX.EnableAttackAlert(true);
    }

    private void DisableCounterWindow()
    {
        enemy.EnableCounterWindow(false);
        enemyVFX.EnableAttackAlert(false);
    }
}
