using Godot;

[GlobalClass]
public partial class Animator : Node
{
  [Export] private AnimationPlayer? _animationPlayer;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Interact"))
      return;

    _animationPlayer!.Play("Zig");
  }
}