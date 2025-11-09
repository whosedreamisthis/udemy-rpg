using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected Player player;
    protected string animBoolName;

    protected Animator anim;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        Debug.Log("Update " + animBoolName);
    }
    
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

}
