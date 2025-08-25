using Godot;
using PixelHunt.Mechanics.Markers;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class LargeRubbish : RigidBody3D, IAimMarkerBearer
{
  [Export] public Marker? AimMarker { get; private set; }
}
