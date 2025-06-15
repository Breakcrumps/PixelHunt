using System.IO;
using Godot;

public partial class EnvInput : LineEdit
{
  private SceneEnvironment? _sceneEnv;

  [Export] private EnvLabel? _envLabel;

  private EnvInput()
  {
    EventBus.Created += node =>
    {
      if (node is SceneEnvironment sceneEnv)
        _sceneEnv = sceneEnv;
    };
  }

  public override void _Ready()
  {
    TextSubmitted += ChangeEnvironment;
  }

  private void ChangeEnvironment(string input)
  {
    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;

    string filepath = $@"Resources\Environment\{input}.tres";

    if (!File.Exists(filepath))
      return;

    _sceneEnv!.Environment = ResourceLoader.Load<Environment>(filepath);
    _envLabel?.UpdateDisplay(_sceneEnv.Environment);
  }
}
