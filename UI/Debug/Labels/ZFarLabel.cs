using Godot;

public partial class ZFarLabel : Label
{
  private ZFarLabel()
  {
    EventBus.Ready += node =>
    {
      if (node is CharacterCamera characterCamera)
        Text = $"{characterCamera.Far}";
    };
  }
}
