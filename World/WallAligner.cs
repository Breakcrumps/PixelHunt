using Godot;
using PixelHunt.Mechanics.Pulse.PulseObeyers;
using PixelHunt.Mechanics.Stasis.StasisObeyers;
using PixelHunt.Static;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class WallAligner : Node
{
  [Export] private LargeRubbish? _wall;
  [Export] private RigidPulseObeyer? _pulseObeyer;
  [Export] private RigidStasisObeyer? _stasisObeyer;
  [Export] private GroundRaycast? _groundRaycast;

  private static Vector3 _desiredOrientation = new(.1f, 0f, 0f);

  public override void _PhysicsProcess(double delta)
  {
    if (_wall is null)
      return;

    if (_pulseObeyer is null || _pulseObeyer.Pulsing)
      return;

    if (_stasisObeyer is null || _stasisObeyer.InStasis)
      return;

    if (_groundRaycast is null || _groundRaycast.IsOnFloor())
      return;

    if (_wall.Rotation.Sum() >= .1f)
      return;

    _wall.Rotation = _wall.Rotation.Lerp(to: _desiredOrientation, weight: 5f * (float)delta);
  }
}
