using Godot;

[GlobalClass]
internal partial class Sun : DirectionalLight3D
{
  public override void _Ready()
  {
    ShadowEnabled = true;
  }
}
