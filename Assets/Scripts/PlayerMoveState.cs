using UnityEngine;

public class PlayerMoveState : EntityState
{
    SpriteRenderer sr;
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        sr = anim.GetComponent<SpriteRenderer>();
    }


    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.moveInput.x < 0)
        {
            sr.flipX = true;
        }
        else if (player.moveInput.x > 0)
        {
            sr.flipX = false;
        }
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
