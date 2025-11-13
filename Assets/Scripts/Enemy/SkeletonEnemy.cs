using UnityEngine;

public class SkeletonEnemy : Enemy, ICounterable
{
    public bool CanBeCountered
    {
        get => canBeStunned;
    }

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
        battleState = new EnemyBattleState(this, stateMachine, "battle");
        deadState = new EnemyDeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }
}
