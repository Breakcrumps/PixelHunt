using Godot;

public partial class UI : Control
{
  public override void _Ready()
  {
    Input.MouseMode = Input.MouseModeEnum.Captured;
  }

  public override void _Input(InputEvent @event)
  {
    if (!@event.IsActionPressed("Pause"))
      return;

    Input.MouseMode ^= Input.MouseModeEnum.Captured;

    GetTree().Paused = !GetTree().Paused;

    AcceptEvent();
  }
}
