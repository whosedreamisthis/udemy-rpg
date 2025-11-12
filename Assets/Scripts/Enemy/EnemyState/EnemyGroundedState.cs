using System;
using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    public EnemyGroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
