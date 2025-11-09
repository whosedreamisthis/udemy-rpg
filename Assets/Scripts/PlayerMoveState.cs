using UnityEngine;

public class PlayerMoveState : EntityState 
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

    }

  public override void Update()
  {
        base.Update();
    
        if (Input.GetKey(KeyCode.G))
        {
            stateMachine.ChangeState(player.idleState);
        }
  }
}
