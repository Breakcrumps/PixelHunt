using Godot;
using PixelHunt.Mechanics.Pulse.PulseObeyers;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class WallRunAligner : Node
{
  [Export] private LargeRubbish? _rubbish;
  [Export] private RigidPulseObeyer? _pulseObeyer;

  private static Vector3 _desiredRotation = new(.1f, 0f, 0f);

  public override void _PhysicsProcess(double delta)
  {
    if (_rubbish is null)
      return;

    if (_pulseObeyer is null || !_pulseObeyer.Pulsing)
      return;

    _rubbish.Rotation = _rubbish.Rotation.Lerp(to: _desiredRotation, weight: 5f * (float)delta);
  }
}
