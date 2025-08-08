using Godot;

namespace PixelHunt.World;

internal sealed partial class Light : StaticBody3D
{
  [Export] private float _lightEnergy = 1f;
  [Export] private float _radius = 5f;

  public override void _Ready()
  {
    OmniLight3D lightSource = GetNode<OmniLight3D>("%LightSource");

    lightSource.LightEnergy = _lightEnergy;
    lightSource.OmniRange = _radius;
  }
}
