using GameSrc.Parents;
using Godot;

namespace GameSrc.Characters.Enemy.Composites;

[GlobalClass]
internal sealed partial class EnemyStateMachine : StateMachine
{
  [Export] private State? _initialState;

  public override void _Ready()
  {
    FillStates();

    if (_initialState is null)
      return;

    _initialState.Enter();
    CurrentState = _initialState;
  }

  internal override void Process(double delta)
  {
    CurrentState?.Process(delta);
  }

  internal override void PhysicsProcess(double delta)
  {
    CurrentState?.PhysicsProcess(delta);
  }
}
