using Godot;
using PixelHunt.Animation;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Player.Composites;

namespace PixelHunt.Mechanics.Push.PushSources;

[GlobalClass]
internal sealed partial class PlayerPushSource : PushSource
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Skeleton3D? _armature;
  [Export] private MoveStateMachine? _moveStateMachine;
  [Export] private PlayerAnimator? _animator;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Push"))
      return;

    _moveStateMachine?.Transition("StopMoveStrategy");
    _animator?.PlayAnimation("Push");
  }

  internal void EmitPush()
  {
    if (_armature is null)
      return;

    Vector3 direction = _armature.GlobalBasis.Z with { Y = 0f };

    EmitPush(new PushParams(direction.Normalized(), Actor: _playerChar));
  }
}
