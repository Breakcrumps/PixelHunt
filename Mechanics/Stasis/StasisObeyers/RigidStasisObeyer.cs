using Godot;
using PixelHunt.Mechanics.Pulse.PulseObeyers;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisObeyers;

[GlobalClass]
internal sealed partial class RigidStasisObeyer : StasisObeyer
{
  [Export] private RigidBody3D? _body;
  [Export] private RigidPulseObeyer? _pulseObeyer;

  private GameTime _stasisTime = GameTime.Zero;

  private bool _inStasis;

  private protected override void ObeyStasis(StasisParams stasisParams)
  {
    if (_pulseObeyer is null)
      return;

    if (_body is null)
      return;

    if (!_pulseObeyer.Pulsing)
      return;

    _pulseObeyer.Pulsing = false;

    _body.Freeze = true;

    _stasisTime = stasisParams.Duration;

    _inStasis = true;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!_inStasis)
      return;
    
    if (_body is null)
      return;
    
    _stasisTime.Frames--;

    if (_stasisTime == GameTime.Zero)
    {
      _inStasis = false;

      _body.Freeze = false;
    }
  }
}
