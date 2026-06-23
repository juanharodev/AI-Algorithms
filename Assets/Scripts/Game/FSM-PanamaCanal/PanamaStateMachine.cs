using UnityEngine;

public class PanamaStateMachine 
{
    public PanamaStateController Controller { get; private set; }
    public PanamaState CurrentState { get; private set; }

    public PanamaStateMachine(PanamaStateController controller)
    {
        Controller = controller;
    }


    public void Initialize(PanamaState initialState)
    {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(PanamaState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    
}
