using System;
using Godot;

[GlobalClass]
public partial class Idle : State
{
  [Export] private Enemy? _enemy;
  [Export] private StateMachine? _stateMachine;

  [ExportGroup("Parameters")]
  [Export] private float _wanderRadius = 10f;

  private Player? _player;

  private readonly Random _random = new();

  private Vector2 _initialPos;

  private Vector2 _moveDirection;
  private float _wanderTime;

  private void RandomiseWander()
  {
    Vector2 currentPos = new(_enemy!.GlobalPosition.X, _enemy.GlobalPosition.Z);

    Vector2 diffVector = _initialPos - currentPos;

    if (diffVector.Length() > _wanderRadius)
      _moveDirection = diffVector.Normalized();
    else
      _moveDirection = new Vector2(RandomDirection(), RandomDirection()).Normalized();

    _wanderTime = RandomTime();
  }

  public override void Enter()
  {
    _player = (Player)GetTree().GetFirstNodeInGroup("Player");

    if (_enemy is null)
      return;

    _initialPos = new(_enemy.GlobalPosition.X, _enemy.GlobalPosition.Z);

    RandomiseWander();
  }

  public override void Process(double delta)
  {
    if (_wanderTime > 0)
      _wanderTime -= (float)delta;
    else
      RandomiseWander();
  }

  public override void PhysicsProcess(double delta)
  {
    if (_enemy is null)
      return;

    Vector2 horizontalVelocity = _moveDirection * _enemy.Speed;

    _enemy.Velocity = new(horizontalVelocity.X, 0f, horizontalVelocity.Y);

    _enemy.AlignBody(delta);

    Vector3 diffVector = _player!.GlobalPosition - _enemy.GlobalPosition;
    Vector2 direction = new(diffVector.X, diffVector.Z);

    if (direction.Length() < 10)
    {
      _stateMachine?.Transition("Follow");
    }
  }

  private float RandomDirection() => _random.NextSingle() * 2 - 1;
  private float RandomTime() => _random.NextSingle() * 4 + 1;
  private bool InBounds()
  {
    Vector2 currentPos = new(_enemy!.GlobalPosition.X, _enemy.GlobalPosition.Z);

    Vector2 diffVector = currentPos - _initialPos;

    return diffVector.Length() < 10f;
  }
}