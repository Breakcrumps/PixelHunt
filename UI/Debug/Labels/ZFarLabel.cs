using Godot;

public partial class ZFarLabel : Label
{
  private ZFarLabel()
  {
    EventBus.Created += node =>
    {
      if (node is CharacterCamera characterCamera)
        Text = $"{characterCamera.Far}";
    };
  }
}
