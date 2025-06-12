using Godot;

[GlobalClass]
public partial class DialogueArea : Area3D
{
  [Export] private string _name = "NO_NAME";

  private bool _active;

  public override void _Ready()
  {
    BodyEntered += body => { if (body is Amogus) _active = true; };
    BodyExited += body => { if (body is Amogus) _active = false; };
  }

  public override void _ShortcutInput(InputEvent @event)
  {
    if (!_active)
      return;

    if (!@event.IsActionPressed("Interact"))
      return;

    GetViewport().SetInputAsHandled();

    EventBus.InitDialogue(_name);
  }
}
