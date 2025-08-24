using Godot;

namespace PixelHunt.Mechanics.Markers;

internal interface ILockOnMarkerBearer
{
  Vector3 GlobalPosition { get; set; }
  
  Marker? LockOnMarker { get; }
}
