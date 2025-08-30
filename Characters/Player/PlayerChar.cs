using PixelHunt.Characters.Player.Composites;
using PixelHunt.Static;
using PixelHunt.Types;
using Godot;

namespace PixelHunt.Characters.Player;

[GlobalClass]
internal sealed partial class PlayerChar : Character
{
  [Export] internal MoveStateMachine? MoveStateMachine { get; private set; }
  [Export] private Skeleton3D? _skeleton;
  [Export] private Camera3D? _camera;

  public override void _Ready()
  {
    GlobalInstances.PlayerChar = this;
    GlobalInstances.PlayerCamera = _camera;

    EnsureStartingRotation();
  }

  public override void _PhysicsProcess(double delta)
  {
    MoveStateMachine?.PhysicsProcess(delta);

    MoveAndSlide();

    // HandleRigidCollision();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    MoveStateMachine?.UnhandledInput(@event);
  }


  internal override void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Damage;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"Player was hit for {attack.Damage}HP, {Health}HP left.");

    MoveStateMachine?.HandlePushback(attackerPos);

    if (Health <= 0)
      Die();
  }

  private void EnsureStartingRotation()
  {
    if (_skeleton is null)
      return;

    if (_skeleton.Rotation.Y is -Mathf.Pi or Mathf.Pi)
      return;

    _skeleton.Rotation = _skeleton.Rotation with { Y = Mathf.Pi };
  }
}
