using Godot;
using System.Collections.Generic;

public abstract partial class StateMachine : Node
{
  protected readonly Dictionary<string, State> _states = [];
  protected State? _currentState;

  protected void FillStates()
  {
    foreach (Node child in GetChildren())
    {
      if (child is State state)
        _states.Add(child.Name, state);
    }
  }

  public virtual void Process(double delta) { }

  public virtual void PhysicsProcess(double delta) { }

  public void Transition(string nextStateName)
  {
    State newState = _states[nextStateName];

    if (newState is null)
      return;

    _currentState?.Exit();

    newState.Enter();
    _currentState = newState;
  }
}