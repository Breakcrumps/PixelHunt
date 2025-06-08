using Godot;

public partial class FpsInput : LineEdit
{
  public override void _Ready()
  {
    TextSubmitted += ChangeFPSLimit;
  }

  private void ChangeFPSLimit(string input)
  {
    if (!input.IsValidInt())
      return;

    int newTargetFps = input.ToInt();

    Engine.MaxFps = newTargetFps;

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;
  }
}
