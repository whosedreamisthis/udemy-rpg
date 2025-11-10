using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected Player player;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    protected float stateTimer;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public virtual void Enter()
    {
        Debug.Log("enter state " + animBoolName);
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public virtual void Exit()
    {
        Debug.Log("exit state " + animBoolName);
        anim.SetBool(animBoolName, false);
    }

    private bool CanDash()
    {
        if (player.wallDetected)
        {
            return false;
        }

        if (stateMachine.currentState == player.dashState)
        {
            return false;
        }
        
        return true;
  }

}
