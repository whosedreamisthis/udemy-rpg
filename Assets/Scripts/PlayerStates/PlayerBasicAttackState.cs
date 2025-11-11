using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private const int FirstCombgoIndex = 1;
    private int comboIndex = 1;
    private const int ComboLimit = 3;
    private int attackDirection;
    private bool comboAttackQueued;

    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDirection =
            (player.moveInput.x != 0) ? ((int)player.moveInput.x) : player.facingDirection;

        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }
        if (triggerCalled)
        {
            HandleStateExit();
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

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void QueueNextAttack()
    {
        if (comboIndex < ComboLimit)
        {
            comboAttackQueued = true;
        }
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
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
}
