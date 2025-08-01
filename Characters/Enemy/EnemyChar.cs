using GameSrc.Characters.Enemy.Composites;
using GameSrc.Characters.Enemy.States;
using GameSrc.Types;
using Godot;

namespace GameSrc.Characters.Enemy;

[GlobalClass]
internal sealed partial class EnemyChar : Character
{
  [Export] private Node3D? _armature;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private PushbackState? _pushbackState;

  public override void _Process(double delta)
  {
    _stateMachine?.Process(delta);
  }

  public override void _PhysicsProcess(double delta)
  {
    _stateMachine?.PhysicsProcess(delta);

    ApplyGravity();

    MoveAndSlide();
  }

  internal void AlignBody(double delta, bool inverse = false)
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    Vector2 refVector = (
      inverse
      ? new Vector2(0, -1)
      : new Vector2(0, 1)
    );

    _armature!.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: horizontalVelocity.AngleTo(refVector),
        weight: 10f * (float)delta
      )
    };
  }

  private void ApplyGravity()
  {
    if (IsOnFloor())
      return;

    Velocity = Velocity with { Y = -10f };
  }

  internal override void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Damage;

    Vector3 pushbackDirection = (GlobalPosition - attackerPos) with { Y = 0f };

    _pushbackState!.PushbackDirection = pushbackDirection.Normalized();

    _stateMachine?.Transition("PushbackState");

    if (Health == 0)
      QueueFree();
  }
}
