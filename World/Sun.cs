using Godot;

[GlobalClass]
internal sealed partial class Sun : DirectionalLight3D
{
  public override void _Ready()
  {
    ShadowEnabled = true;
  }
}
