using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Enemy.Composites;

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
}
