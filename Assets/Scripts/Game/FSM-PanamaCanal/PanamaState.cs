using UnityEngine;

public abstract class PanamaState
{

    protected PanamaStateMachine stateMachine;
    protected PanamaStateController controller;


    public PanamaState(PanamaStateMachine _stateMachine, PanamaStateController _controller)
    {
        stateMachine = _stateMachine;
        controller = _controller;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
