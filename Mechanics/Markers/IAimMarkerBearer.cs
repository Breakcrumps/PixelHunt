using Godot;

namespace PixelHunt.Mechanics.Markers;

internal interface IAimMarkerBearer
{
  Vector3 GlobalPosition { get; set; }
  Marker? AimMarker { get; }

  void QueueFree();
}
