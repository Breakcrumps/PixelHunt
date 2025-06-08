using Godot;

public partial class ZFar : Label
{
  public override void _Ready()
  {
    EventBus.ZFarChange += UpdateDisplay;
  }

  private void UpdateDisplay(int newZFar)
  {
    Text = $"ZFar: {newZFar}";
  }
}
