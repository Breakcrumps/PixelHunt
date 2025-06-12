using Godot;

public partial class ZFarLabel : Label
{
  public override void _Ready()
  {
    EventBus.ZFarChange += UpdateDisplay;
  }

  private void UpdateDisplay(float newZFar)
  {
    Text = $"ZFar: {newZFar}";
  }
}
