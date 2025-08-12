using PixelHunt.Animation;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Player;
using PixelHunt.Parents;
using Godot;
using PixelHunt.Static;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class FollowState : State
{
  [Export] private EnemyChar? _enemyChar;

  [Export] private EnemyAligner? _enemyAligner;

  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  [Export] private EnemyStateMachine? _stateMachine;

  [Export] private VisionCone? _visionArea;
  [Export] private SoundArea? _soundArea;

  [Export] private NavigationAgent3D? _navigationAgent;

  internal PlayerChar? PlayerChar { private get; set; }

  internal override void Enter()
  {
    _visionArea?.DisableSearch();
    _soundArea?.DisableSearch();
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_enemyChar is null)
      return;

    Vector3 diffVector = PlayerChar!.GlobalPosition - _enemyChar.GlobalPosition;
    Vector2 distance = new(diffVector.X, diffVector.Z);

    if (distance.Length() < 2f)
    {
      _stateMachine?.Transition("AttackState");
      return;
    }

    if (distance.Length() > 30f)
    {
      _stateMachine?.Transition("IdleState");
      _visionArea?.EnableSearch();
      return;
    }

    Vector2 direction = DetermineDirection();

    _animator?.PlayAnimation(
      direction == Vector2.Zero
      ? "Idle"
      : "Walk"
    );

    Vector2 velocity = direction * _animHelper!.Speed;

    _enemyChar.Velocity = new Vector3(velocity.X, 0f, velocity.Y);

    _enemyAligner?.AlignBodyToVelocity(delta);
  }

  private Vector2 DetermineDirection()
  {
    if (_navigationAgent is null)
      return Vector2.Zero;

    if (PlayerChar is null)
      return Vector2.Zero;

    if (_enemyChar is null)
      return Vector2.Zero;

    _navigationAgent.TargetPosition = PlayerChar.GlobalPosition;

    if (!_navigationAgent.IsTargetReachable())
      return Vector2.Zero;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print("Navigation was not finished.");

    Vector3 nextDirection = _navigationAgent.GetNextPathPosition() - _enemyChar.GlobalPosition;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print(_navigationAgent.GetNextPathPosition());

    return new Vector2(nextDirection.X, nextDirection.Z).Normalized();
  }
}
