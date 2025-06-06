using Godot;

public partial class DialogueArea : Area3D
{
  private bool _active;

  public override void _Ready()
  {
    BodyEntered += (_) => { _active = true; };
    BodyExited += (_) => { _active = false; };
  }

  public override void _Input(InputEvent @event)
  {
    if (!_active)
      return;

    if (!Input.IsActionJustPressed("Interact"))
      return;

    EventBus.InitDialogue(GetParent().Name);
  }
}
