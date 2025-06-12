using Godot;

public partial class PhysFpsInput : LineEdit
{
  [Export] private Label? _physFpsLabel;

  public override void _Ready()
  {
    _physFpsLabel!.Text = $"{Engine.PhysicsTicksPerSecond}";

    TextSubmitted += ChangePhysFps;
  }

  private void ChangePhysFps(string input)
  {
    if (!input.IsValidInt())
      return;

    int newPhysFps = input.ToInt();

    Engine.PhysicsTicksPerSecond = newPhysFps;

    _physFpsLabel!.Text = $"{Engine.PhysicsTicksPerSecond}";

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;
  }
}
