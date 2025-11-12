using UnityEngine;

public class EnemyBattleState : EnemyState
{
    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Enemy Battle State");
    }

    public override void Update()
    {
        base.Update();
    }
}
