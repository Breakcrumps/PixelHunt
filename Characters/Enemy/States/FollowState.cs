using GameSrc.Animation;
using GameSrc.Characters.Enemy.Composites;
using GameSrc.Characters.Player;
using GameSrc.Parents;
using Godot;

namespace GameSrc.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class FollowState : State
{
  [Export] EnemyChar? _enemyChar;
  [Export] private Animator? _animator;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private VisionCone? _visionArea;
  [Export] private SoundArea? _soundArea;
  [Export] private AnimationHelper? _animHelper;

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
    Vector2 direction = new(diffVector.X, diffVector.Z);

    if (direction.Length() < 2f)
    {
      _stateMachine?.Transition("AttackState");
      return;
    }

    if (direction.Length() > 30f)
    {
      _stateMachine?.Transition("IdleState");
      _visionArea?.EnableSearch();
      return;
    }

    Vector2 velocity = direction.Normalized() * _animHelper!.Speed;

    _enemyChar.Velocity = _enemyChar.Velocity with
    {
      X = velocity.X,
      Z = velocity.Y
    };

    _animator?.PlayAnimation("Walk");

    _enemyChar.AlignBody(delta);
  }
}
