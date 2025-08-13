using Godot;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Mechanics.Pulse;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class PulseState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _enemyStateMachine;
  [Export] private FollowState? _followState;

  internal PulseParams? PulseParams { private get; set; }

  internal override void Enter()
  {
    if (_enemyChar is null)
      return;

    _enemyChar.Velocity = _enemyChar.Velocity with { Y = 100f };
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_enemyChar is null)
      return;

    if (_followState is null)
      return;

    if (!_enemyChar.IsOnFloor())
      return;

    _followState.Target = PulseParams?.Actor;
    _enemyStateMachine?.Transition("FollowState");
  }
}
