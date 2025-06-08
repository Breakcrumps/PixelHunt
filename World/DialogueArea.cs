using Godot;

[GlobalClass]
public partial class DialogueArea : Area3D
{
  [Export] private string _name = "NO_NAME";

  private bool _active;

  public override void _Ready()
  {
    BodyEntered += _ => { _active = true; };
    BodyExited += _ => { _active = false; };
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (!_active)
      return;

    if (!@event.IsActionPressed("Interact"))
      return;

    EventBus.InitDialogue(_name);
  }
}
