using Godot;

public partial class JitterFixInput : LineEdit
{
  [Export] private Label _jitterFixLabel;

  public override void _Ready()
  {
    _jitterFixLabel.Text = $"{Engine.PhysicsJitterFix}";

    TextSubmitted += ChangeJitterFix;
  }

  private void ChangeJitterFix(string input)
  {
    if (!input.IsValidFloat())
      return;

    double newJitterFix = input.ToFloat();

    Engine.PhysicsJitterFix = newJitterFix;

    _jitterFixLabel.Text = $"{Engine.PhysicsJitterFix}";

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;
  }
}
