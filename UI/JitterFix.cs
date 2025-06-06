using Godot;

public partial class JitterFix : Label
{
  [Export] private JitterFixInput _input;

  public override void _Ready()
  {
    UpdateDisplay();

    _input.JitterFixChanged += UpdateDisplay;
  }

  public override void _ExitTree()
  {
    _input.JitterFixChanged -= UpdateDisplay;
  }

  private void UpdateDisplay()
  {
    Text = $"{Engine.PhysicsJitterFix}";
  }
}
