using Godot;

public partial class ZFarLabel : Label
{
  public override void _Ready()
  {
    Camera3D characterCamera = (Camera3D)GetTree().GetFirstNodeInGroup("CharacterCamera");

    Text = $"{characterCamera.Far}";
  }
}
