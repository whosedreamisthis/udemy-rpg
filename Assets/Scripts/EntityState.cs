using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected Player player;
    protected string stateName;

    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        Debug.Log("Enter " + stateName);
    }

    public virtual void Update()
    {
        Debug.Log("Update " + stateName);
    }
    
    public virtual void Exit()
    {
        Debug.Log("Exit " + stateName);
    }

}
