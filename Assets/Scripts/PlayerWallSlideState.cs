using UnityEngine;

public class PlayerWallSlideState : EntityState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();

        if (player.wallDetected == false)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.groundDetected)
        {
            // 
            player.SetVelocity(0, rb.linearVelocity.y);
            stateMachine.ChangeState(player.idleState);
            // player.Flip();

        }
    }
  
  private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
    else
    {
      player.SetVelocity(player.moveInput.x,rb.linearVelocity.y * player.wallSlideSlowMultiplier);
    }
    
  }
}
