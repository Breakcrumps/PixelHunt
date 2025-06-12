using System.IO;
using Godot;

public partial class EnvInput : LineEdit
{
  [Export] private Label _envLabel;

  public override void _Ready()
  {
    _envLabel.Text = "Light";

    TextSubmitted += ChangeEnvironment;
  }

  private void ChangeEnvironment(string input)
  {
    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;

    string filepath = $@"Environment\{input}.tres";

    if (!File.Exists(filepath))
      return;

    _envLabel.Text = input;

    EventBus.ChangeEnvironment(input);
  }
}
