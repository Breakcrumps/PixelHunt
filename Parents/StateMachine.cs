using Godot;
using System.Collections.Generic;

namespace GameSrc.Parents;

internal abstract partial class StateMachine : Node
{
  private protected Dictionary<string, State> States { get; } = [];
  private protected State? CurrentState { get; set; }

  private protected void FillStates()
  {
    foreach (Node child in GetChildren())
    {
      if (child is State state)
        States.Add(child.Name, state);
    }
  }

  internal virtual void Process(double delta) { }

  internal virtual void PhysicsProcess(double delta) { }

  internal void Transition(string nextStateName)
  {
    State newState = States[nextStateName];

    if (newState is null)
      return;

    CurrentState?.Exit();

    newState.Enter();
    CurrentState = newState;
  }
}