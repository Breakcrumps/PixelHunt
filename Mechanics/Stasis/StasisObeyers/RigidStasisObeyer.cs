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

  internal bool InStasis { get; private set; }

  public override void _Ready()
  {
    base._Ready();
    
    if (_body is null)
      return;
    
    _body.FreezeMode = RigidBody3D.FreezeModeEnum.Kinematic;
  }

  private protected override void ObeyStasis(StasisParams stasisParams)
  {
    if (_pulseObeyer is null)
      return;

    if (_body is null)
      return;

    if (!_pulseObeyer.Pulsing)
      return;

    _pulseObeyer.Pulsing = false;

    _stasisTime = stasisParams.Duration;

    InStasis = true;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!InStasis)
      return;
    
    if (_body is null)
      return;

    if (_pulseObeyer is null)
      return;
    
    _stasisTime.Frames--;

    if (_stasisTime == GameTime.Zero)
    {
      InStasis = false;

      _body.GravityScale = _pulseObeyer.InitialGravityScale;
    }
  }
}
