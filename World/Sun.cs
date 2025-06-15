using Godot;

[GlobalClass]
public partial class Sun : DirectionalLight3D
{
  public override void _Ready()
  {
    ShadowEnabled = true;

    EventBus.NotifyReady(this);
  }
}
