using Godot;

internal partial class FpsLabel : Label
{
  public override void _Process(double delta)
  {
    double fps = Engine.GetFramesPerSecond();
    Text = $"FPS: {fps}";
  }
}
