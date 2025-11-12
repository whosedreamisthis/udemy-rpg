using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private EnemyVFX enemyVFX;

    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyVFX = enemy.GetComponent<EnemyVFX>();
    }

    public override void Enter()
    {
        base.Enter();
        enemyVFX.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
        stateTimer = enemy.stunnedDuration;

        rb.linearVelocity = new Vector2(
            enemy.stunnedVelocity.x * -enemy.facingDir,
            enemy.stunnedVelocity.y
        );
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
