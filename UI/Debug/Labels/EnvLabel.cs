using Godot;

public partial class EnvLabel : Label
{
  public void UpdateDisplay()
  {
    SceneEnvironment env = (SceneEnvironment)GetTree().GetFirstNodeInGroup("SceneEnv");
    Environment envResource = env.Environment;

    string text = envResource.ResourcePath
      .Split('/')[^1]
      .Split('.')[0];

    Text = text;
  }
}
