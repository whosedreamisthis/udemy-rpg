using UnityEngine;

public class SkeletonEnemy : Enemy
{
    public SkeletonEnemy() { }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(this, stateMachine, "idle");
        moveState = new EnemyMoveState(this, stateMachine, "move");
        attackState = new EnemyAttackState(this, stateMachine, "attack");
    }
}
