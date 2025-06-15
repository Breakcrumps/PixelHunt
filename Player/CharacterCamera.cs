using Godot;

public partial class CharacterCamera : Camera3D
{
  public override void _Ready()
  {
    EventBus.NotifyCreation(this);
  }
}
