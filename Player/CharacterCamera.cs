using Godot;

public partial class CharacterCamera : Camera3D
{
  public override void _Ready()
  {
    EventBus.ZFarChange += newZFar => Far = newZFar;
    
    EventBus.ChangeZFar(Far);
  }
}
