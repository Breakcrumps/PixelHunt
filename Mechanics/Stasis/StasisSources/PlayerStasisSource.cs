using Godot;
using PixelHunt.Characters.Player;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisSources;

[GlobalClass]
internal sealed partial class PlayerStasisSource : StasisSource
{
  [Export] private PlayerChar? _playerChar;
  
  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Stasis"))
      return;

    if (_playerChar is null)
      return;

    EmitStasis(new StasisParams(_playerChar, GameTime.Frame * 180));
  }
} 
