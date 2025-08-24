using Godot;
using PixelHunt.Mechanics.Markers;
using PixelHunt.Static;
using static Godot.Mathf;

namespace PixelHunt.Mechanics.Aim;

[GlobalClass]
internal sealed partial class AimArea : Area3D
{
  private IAimMarkerBearer? _target;

  public override void _PhysicsProcess(double delta)
  {
    if (!Input.IsActionPressed("Aim"))
      return;

    RelinquishTarget();
    _target = DetermineTarget();
  }

  private IAimMarkerBearer? DetermineTarget()
  {
    IAimMarkerBearer? target = null;

    float bestError = float.PositiveInfinity;
    float bestDistance = float.PositiveInfinity;

    foreach (Node node in GetOverlappingBodies())
    {
      if (node is not IAimMarkerBearer candidate)
        continue;

      if (
        GetError(candidate) < bestError
        || GetError(candidate) == bestError && GetDistance(candidate) < bestDistance
      )
      {
        target = candidate;

        bestError = GetError(candidate);
        bestDistance = GetDistance(candidate);
      }
    }

    target?.AimMarker?.ShowMarker();
    return target;
  }

  private void RelinquishTarget()
  {
    _target?.AimMarker?.HideMarker();
    _target = null;
  }

  private float GetError(IAimMarkerBearer candidate)
  {
    Vector3 hypotenuse = candidate.GlobalPosition - GlobalPosition;
    Vector3 cathetus = -GlobalBasis.Z;

    return hypotenuse.Length() * Sin(hypotenuse.AngleTo(cathetus));
  }

  private float GetDistance(IAimMarkerBearer candidate)
    => (candidate.GlobalPosition - GlobalPosition).Length();

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!Input.IsActionPressed("Aim"))
      return;

    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    Rotation = new Vector3(0f, -(mouseMotion.Relative.Angle().Round(digits: 1) + Pi / 2f), 0f);
  }
}
