using Godot;
using PixelHunt.Mechanics.Pulse.PulseObeyers;

using static Godot.Mathf;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class WallRunAligner : Node
{
  [Export] private LargeRubbish? _wall;
  [Export] private RigidPulseObeyer? _pulseObeyer;

  private static Vector3 _horizontalOrientation = new(Pi / 2f, 0f, 0f);

  private bool _flipped = false;

  public override void _PhysicsProcess(double delta)
  {
    if (_wall is null)
      return;

    if (_pulseObeyer is null || !_pulseObeyer.Pulsing)
      return;

    Vector3 desiredOrientation = _flipped ? _horizontalOrientation : Vector3.Zero;

    _wall.Rotation = _wall.Rotation.Lerp(to: desiredOrientation, weight: 5f * (float)delta);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("FlipWalls"))
      _flipped = !_flipped;
  }
}
