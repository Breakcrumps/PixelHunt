using Godot;

public partial class PhysFpsCounter : Label
{
  [Export] private PhysFpsInput _input;

  public override void _Ready()
  {
    UpdateDisplay();

    _input.PhysFpsChanged += UpdateDisplay;
  }

  public override void _ExitTree()
  {
    _input.PhysFpsChanged -= UpdateDisplay;
  }

  private void UpdateDisplay()
  {
    Text = $"{Engine.PhysicsTicksPerSecond}";
  }
}
