using Godot;

public partial class EnvLabel : Label
{
  private EnvLabel()
  {
    EventBus.Ready += node =>
    {
      if (node is SceneEnvironment env)
        UpdateDisplay(env.Environment);
    };
  }

  public void UpdateDisplay(Environment envResource)
  {
    string text = envResource.ResourcePath
      .Split('/')[^1]
      .Split('.')[0];

    Text = text;
  }
}
