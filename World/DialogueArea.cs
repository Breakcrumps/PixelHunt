using Godot;

[GlobalClass]
public partial class DialogueArea : Area3D
{
  private DialogueBox? _dialogueBox;
  
  [Export] private string _name = "NO_NAME";

  private bool _active;

  private DialogueArea()
  {
    EventBus.Ready += node
      => { if (node is DialogueBox dialogueBox) _dialogueBox = dialogueBox; };
  }

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

    _dialogueBox?.InitDialogue(_name);
  }
}
