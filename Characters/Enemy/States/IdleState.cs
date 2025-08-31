using System;
using PixelHunt.Animation;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class IdleState : State
{
  [Export] bool _canWander = true;

  [Export] private EnemyChar? _enemyChar;

  [Export] private EnemyAligner? _enemyAligner;

  [Export] private Animator? _animator;
  [Export] private AnimationPlayer? _animPlayer;
  [Export] private AnimationHelper? _animHelper;

  [Export] private EnemyStateMachine? _stateMachine;

  [Export] private VisionCone? _visionArea;
  [Export] private SoundArea? _soundArea;

  [ExportGroup("Parameters")]
  [Export] private float _wanderRadius = 10f;

  private readonly Random _random = new();

  private Vector2 _initialPos;

  private Vector2 _moveDirection;
  private float _wanderTime;
  
  internal override void Enter()
  {
    _visionArea?.EnableSearch();
    _soundArea?.EnableSearch();

    if (_enemyChar is null)
      return;

    _initialPos = new Vector2(_enemyChar.GlobalPosition.X, _enemyChar.GlobalPosition.Z);

    RandomiseWander();
  }

  private void RandomiseWander()
  {
    Vector2 currentPos = new(_enemyChar!.GlobalPosition.X, _enemyChar.GlobalPosition.Z);

    Vector2 diffVector = _initialPos - currentPos;

    if (diffVector.Length() > _wanderRadius)
    {
      _moveDirection = diffVector.Normalized();
      _wanderTime = RandomTime();
      return;
    }

    _moveDirection = new Vector2(RandomDirection(), RandomDirection()).Normalized();

    if (_animator is null)
      return;

    if (_moveDirection == Vector2.Zero)
    {
      _animator.PlayAnimation("Idle");
      _wanderTime = _animPlayer!.GetAnimation("Idle1").Length;
    }
    else
    {
      _animator.PlayAnimation("Walk");
      _wanderTime = RandomTime();
    }
  }

  internal override void Process(double delta)
  {
    if (_wanderTime > 0)
      _wanderTime -= (float)delta;
    else
      RandomiseWander();
  }

  internal override void PhysicsProcess(double delta)
  {
    if (!_canWander)
      return;

    if (_enemyChar is null)
      return;

    Vector2 horizontalVelocity = _moveDirection * _animHelper!.Speed;

    _enemyChar.Velocity = _enemyChar.Velocity with
    {
      X = horizontalVelocity.X,
      Z = horizontalVelocity.Y
    };

    _enemyAligner?.AlignBodyToVelocity(delta);
  }

  private float RandomDirection() => _random.NextSingle() * 2f - 1f;
  private float RandomTime() => _random.NextSingle() * 4f + 5f;
  private bool InBounds()
  {
    Vector2 currentPos = new(_enemyChar!.GlobalPosition.X, _enemyChar.GlobalPosition.Z);

    Vector2 diffVector = currentPos - _initialPos;

    return diffVector.Length() < 10f;
  }
}
