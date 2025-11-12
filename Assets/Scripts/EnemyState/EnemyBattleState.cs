using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private Transform player;
    public float lastTimeWasInBattle;

    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        if (player == null)
        {
            player = enemy.PlayerDetection().transform;
        }

        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(
                -directionToPlayer() * enemy.retreatVelocity.x,
                enemy.retreatVelocity.y
            );
            enemy.HandleFlip(directionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection() == true)
        {
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (WithinAttackRange() && enemy.PlayerDetection() == true)
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }
        else
        {
            enemy.SetVelocity(
                directionToPlayer() * enemy.battleMoveSpeed,
                enemy.rb.linearVelocity.y
            );
        }

        // float directionToPlayer = player.position.x - enemy.transform.position.x;
        // float moveDir = directionToPlayer > 0 ? 1f : -1f;

        // enemy.SetVelocity(moveDir * enemy.battleMoveSpeed, enemy.rb.linearVelocity.y);
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool WithinAttackRange() => DistanceToPlayer() <= enemy.attackDistance;

    private bool BattleTimeIsOver() => Time.time >= lastTimeWasInBattle + enemy.battleTimeDuration;

    private bool ShouldRetreat() => DistanceToPlayer() <= enemy.minRetreatDistance;

    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
        // Vector2.Distance(enemy.transform.position, player.position);
    }

    private int directionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x - enemy.transform.position.x > 0 ? 1 : -1;
    }
}
