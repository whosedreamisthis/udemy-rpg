using UnityEngine;

public class PlayerWallSlideState : EntityState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update(); 
    
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            // player.Flip();
        }
  }
}
