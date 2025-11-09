using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

    }

    public override void Enter ()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);

        if (player.wallDetected)
        {   
            player.Flip();
        }
    }
    public override void Update()
    {
        base.Update();
        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    
    
    
}
