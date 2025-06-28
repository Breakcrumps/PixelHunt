using Godot;
using System.Collections.Generic;

internal abstract partial class StateMachine : Node
{
  protected readonly Dictionary<string, State> _states = [];
  protected State? _currentState;

  private protected void FillStates()
  {
    foreach (Node child in GetChildren())
    {
      if (child is State state)
        _states.Add(child.Name, state);
    }
  }

  internal virtual void Process(double delta) { }

  internal virtual void PhysicsProcess(double delta) { }

  internal void Transition(string nextStateName)
  {
    State newState = _states[nextStateName];

    if (newState is null)
      return;

    _currentState?.Exit();

    newState.Enter();
    _currentState = newState;
  }
}