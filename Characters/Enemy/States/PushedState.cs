using Godot;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Parents;
using PixelHunt.Static;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class PushedState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private FollowState? _followState;
  [Export] private GravityComposite? _gravityComposite;

  [Export] private int _pushFrames = 10;

  internal Vector3 InitialVelocity { private get; set; }
  internal Character? Actor { private get; set; }

  private Vector3 _velocity;

  internal override void Enter()
  {
    _velocity = InitialVelocity;

    if (_gravityComposite is not null)
      _gravityComposite.CanGravitate = true;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_followState is null)
      return;
    
    if (_enemyChar is null)
      return;

    _enemyChar.Velocity = new Vector3(_velocity.X, _enemyChar.Velocity.Y, _velocity.Z);

    if (_enemyChar.IsOnFloor())
      _velocity -= InitialVelocity * 1f / _pushFrames;

    if (_velocity.IsRoughlyZero(tolerance: .1f))
    {
      _followState.Target = Actor;
      _stateMachine?.Transition("FollowState");
    }
  }
}
