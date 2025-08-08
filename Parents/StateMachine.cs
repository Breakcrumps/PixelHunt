using System.Collections.Generic;
using Godot;

namespace PixelHunt.Parents;

internal abstract partial class StateMachine : Node
{
  private protected Dictionary<string, State> States { get; } = [];
  private protected State? CurrentState { get; set; }

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
