using Godot;

public partial class ZFarInput : LineEdit
{
  private Camera3D? _characterCamera;
  
  [Export] private ZFarLabel? _zFarLabel;

  public override void _Ready()
  {
    _characterCamera = (Camera3D)GetTree().GetFirstNodeInGroup("CharacterCamera");

    TextSubmitted += ChangeZFar;
  }

  private void ChangeZFar(string input)
  {
    if (!input.IsValidInt())
      return;

    int newZFar = input.ToInt();

    _characterCamera!.Far = newZFar;
    _zFarLabel!.Text = $"{_characterCamera.Far}";

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;
  }
}
