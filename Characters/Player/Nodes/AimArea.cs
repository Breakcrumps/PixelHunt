using Godot;
using PixelHunt.Mechanics.Markers;

[GlobalClass]
internal sealed partial class AimArea : Area3D
{
  public override void _Ready()
  {
    BodyEntered += node =>
    {
      if (node is IAimMarkerBearer target)
        target.AimMarker?.ShowMarker();
    };

    BodyExited += node =>
    {
      if (node is IAimMarkerBearer target)
        target.AimMarker?.HideMarker();
    };
  }
}
