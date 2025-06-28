using Godot;

internal partial class EnvLabel : Label
{
  internal void UpdateDisplay()
  {
    SceneEnvironment env = (SceneEnvironment)GetTree().GetFirstNodeInGroup("SceneEnv");
    Environment envResource = env.Environment;

    string text = envResource.ResourcePath
      .Split('/')[^1]
      .Split('.')[0];

    Text = text;
  }
}
