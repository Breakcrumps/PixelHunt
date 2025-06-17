using Godot;

[GlobalClass]
public partial class AttackManager : Node
{
  [Export] private AnimationPlayer? _animPlayer;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Attack"))
      return;

    _animPlayer!.Play("Attack");
  }
}