using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
    private float attackVelocityTimer;
    private const int FirstCombgoIndex = 1;
    private int comboIndex = 1;
    private const int ComboLimit = 3;
    private float lastTimeAttacked;

    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
        ResetComboIndexIfNeeded();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        if (comboIndex > ComboLimit)
        {
            comboIndex = FirstCombgoIndex;
        }

        lastTimeAttacked = Time.time;
    }

    void ResetComboIndexIfNeeded()
    {
        if (Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstCombgoIndex;
        }
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        attackVelocityTimer = player.attcakVelocityDuration;
        int attackVelocityIndex = comboIndex - FirstCombgoIndex;
        Vector2 attackVelocity = player.attackVelocity[attackVelocityIndex];
        player.SetVelocity(attackVelocity.x * player.facingDirection, attackVelocity.y);
    }
}
