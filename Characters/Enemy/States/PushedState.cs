using Godot;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class PushedState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private FollowState? _followState;

  [Export] private int _pushFrames = 10;

  internal Vector3 InitialVelocity { private get; set; }
  internal Character? Actor { private get; set; }

  private Vector3 _velocity;

  internal override void Enter()
    => _velocity = InitialVelocity;

  internal override void PhysicsProcess(double delta)
  {
    if (_followState is null)
      return;
    
    if (_enemyChar is null)
      return;

    _enemyChar.Velocity = _velocity;

    _velocity -= InitialVelocity * 1f / _pushFrames;

    if (_velocity.IsZeroApprox())
    {
      _followState.Target = Actor;
      _stateMachine?.Transition("FollowState");
    }
  }
}
