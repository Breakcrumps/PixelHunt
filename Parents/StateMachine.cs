using System.Collections.Generic;
using Godot;

namespace PixelHunt.Parents;

[GlobalClass]
internal abstract partial class StateMachine : Node
{
  private protected Dictionary<string, State> States { get; } = [];
  internal State? CurrentState { get; private protected set; }

  private protected void FillStates()
  {
    foreach (Node child in GetChildren())
    {
      if (child is State state)
      {
        States.Add(child.Name, state);
      }
    }
  }

  internal void Process(double delta) => CurrentState?.Process(delta);

  internal void PhysicsProcess(double delta) => CurrentState?.PhysicsProcess(delta);

  internal void UnhandledInput(InputEvent @event) => CurrentState?.UnhandledInput(@event);

  internal virtual void Transition(string nextStateName)
  {
    State newState = States[nextStateName];

    if (newState is null)
      return;

    if (!newState.Condition())
      return;

    CurrentState?.Exit();

    newState.Enter();
    CurrentState = newState;
  }
}
