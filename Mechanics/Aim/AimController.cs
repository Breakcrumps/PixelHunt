using Godot;

namespace PixelHunt.Mechanics.Aim;

[GlobalClass]
internal sealed partial class AimController : Node
{
  [Export] private Area3D? _aimArea;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!Input.IsActionPressed("Aim"))
      return;

    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    if (_aimArea is null)
      return;

    _aimArea.Rotation = _aimArea.Rotation with { Y = -(mouseMotion.Relative.Angle() + Mathf.Pi / 2f) };
  }
}
