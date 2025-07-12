using GameSrc.Animation;
using GameSrc.Characters.Player.Composites;
using GameSrc.Static;
using GameSrc.Types;
using Godot;

namespace GameSrc.Characters.Player;

[GlobalClass]
internal sealed partial class PlayerChar : Character
{
  [Export] private CameraController? _cameraController;
  [Export] private PlayerAnimator? _animator;
  [Export] private MoveStateMachine? _moveStateMachine;

  public override void _PhysicsProcess(double delta)
  {
    _moveStateMachine?.PhysicsProcess(delta);

    MoveAndSlide();
  }

  internal override void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Damage;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"Player was hit for {attack.Damage}HP, {Health}HP left.");

    _moveStateMachine?.HandlePushback(attackerPos);

    if (Health <= 0)
      Die();
  }
}
