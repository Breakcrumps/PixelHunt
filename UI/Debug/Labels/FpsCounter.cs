using Godot;

public partial class FpsCounter : Label
{
  public override void _Process(double delta)
  {
    double fps = Engine.GetFramesPerSecond();
    Text = $"FPS: {fps}";
  }
}
