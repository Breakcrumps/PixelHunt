using Godot;
using PixelHunt.Characters.Player;

namespace PixelHunt.Mechanics.Pulse.PulseSources;

[GlobalClass]
internal sealed partial class PlayerPulseSource : PulseSource
{
  [Export] private PlayerChar? _playerChar;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Pulse"))
      return;

    if (_playerChar is null)
      return;

    EmitPulse(new PulseParams { Actor = _playerChar });
  }
}
