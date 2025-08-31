using Godot;
using PixelHunt.Animation;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisSources;

[GlobalClass]
internal sealed partial class PlayerStasisSource : StasisSource
{
  [Export] private PlayerChar? _playerChar;
  [Export] private MoveStateMachine? _moveStateMachine;
  [Export] private PlayerAnimator? _animator;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Stasis"))
      return;

    _moveStateMachine?.Transition("StopMoveStrategy");
    _animator?.PlayAnimation("Stasis");
  }

  internal void EmitStasis()
  {
    if (_playerChar is null)
      return;
    
    EmitStasis(new StasisParams(Actor: _playerChar, Duration: GameTime.Second * 8));
  }
} 
