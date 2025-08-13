using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Enemy.States;
using PixelHunt.Types;
using Godot;

namespace PixelHunt.Characters.Enemy;

[GlobalClass]
internal sealed partial class EnemyChar : Character
{
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

  private void ApplyGravity()
  {
    if (IsOnFloor())
      return;

    Velocity = Velocity with { Y = Velocity.Y - 10f };
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
