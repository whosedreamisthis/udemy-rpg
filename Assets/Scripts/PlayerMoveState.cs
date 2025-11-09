using UnityEngine;

public class PlayerMoveState : EntityState 
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

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
            anim.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (player.moveInput.x > 0)
        {
            anim.GetComponent<SpriteRenderer>().flipX = false;
        }
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
  }
}
