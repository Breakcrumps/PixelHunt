using Godot;

public partial class UI : Control
{
  [Export] private CharacterBody3D _amogus;
  [Export] private Label _zFar;
  [Export] private VBoxContainer _vBox;

  public override void _Ready()
  {
    Input.MouseMode = Input.MouseModeEnum.Captured;

    Camera3D camera = _amogus.GetNode<Camera3D>("%Camera3D");

    _zFar.Text = $"Render distance: {camera.Far}";

    EventBus.DialogueInit += InitDialogue;
  }

  public override void _Input(InputEvent @event)
  {
    if (Input.IsActionJustPressed("Perspective"))
    {
      Input.MouseMode ^= Input.MouseModeEnum.Captured;
    }
  }

  public void InitDialogue(string message)
  {
    GD.Print(message);
  }
}
