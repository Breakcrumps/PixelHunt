using Godot;

public partial class ZFarInput : LineEdit
{
  private CharacterCamera? _characterCamera;
  
  [Export] private ZFarLabel? _zFarLabel;

  private ZFarInput()
  {
    EventBus.Ready += node =>
    {
      if (node is CharacterCamera characterCamera)
        _characterCamera = characterCamera;
    };
  }

  public override void _Ready()
  {
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
