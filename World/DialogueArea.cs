using Godot;

public partial class DialogueArea : Area3D
{
  private bool _active;

  public override void _Ready()
  {
    BodyEntered += (_) => { _active = true; };
    BodyExited += (_) => { _active = false; };
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!_active)
      return;

    if (!@event.IsActionPressed("Interact"))
      return;

    EventBus.InitDialogue(GetParent().Name);
  }
}
