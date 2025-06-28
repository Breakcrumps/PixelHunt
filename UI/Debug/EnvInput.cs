using System.IO;
using Godot;

internal partial class EnvInput : LineEdit
{
  private SceneEnvironment? _sceneEnv;

  [Export] private EnvLabel? _envLabel;

  public override void _Ready()
  {
    _sceneEnv = (SceneEnvironment)GetTree().GetFirstNodeInGroup("SceneEnv");

    TextSubmitted += ChangeEnvironment;
  }

  private void ChangeEnvironment(string input)
  {
    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;

    if (_envLabel is null)
    {
      GD.Print(_envLabel);
      return;
    }

    if (input == "Delete")
    {
      _envLabel?.Free();
      return;
    }

    string filepath = $@"Resources\Environment\{input}.tres";

    if (!File.Exists(filepath))
      return;

    _sceneEnv!.Environment = ResourceLoader.Load<Environment>(filepath);
    _envLabel?.UpdateDisplay();
  }
}
