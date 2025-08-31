using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Enemy.Composites;

[GlobalClass]
internal sealed partial class EnemyStateMachine : StateMachine
{
  [Export] private Label3D? _tempLabel;
  [Export] private State? _initialState;

  internal bool CanTransition { private get; set; } = true;

  public override void _Ready()
  {
    FillStates();

    if (_initialState is null)
      return;

    _initialState.Enter();
    CurrentState = _initialState;
  }

  internal override void Transition(string nextStateName)
  {
    if (!CanTransition)
      return;

    base.Transition(nextStateName);

    if (_tempLabel is not null)
      _tempLabel.Text = CurrentState?.Name;
  }
}
