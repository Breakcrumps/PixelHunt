using Godot;

public partial class ZigAnimator : AnimationPlayer
{
  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Interact"))
      return;

    Stop();
    
    Play("Zig");
  }
}
