using System.Collections.Generic;
using Godot;

public partial class DialogueBox : CanvasLayer
{
  private List<string> _lines = [];

  [Export] private Label _label;

  public override void _Ready()
  {
    Hide();

    EventBus.DialogueInit += InitDialogue;
  }

  public override void _Input(InputEvent @event)
  {
    if (!Visible)
      return;

    if (!Input.IsActionJustPressed("Confirm"))
      return;

    NextLine();
  }

  private void InitDialogue(string source)
  {
    GetTree().Paused = true;

    Show();

    _lines = [.. DialogueManager.Dialogue[source]];

    ShowText();
  }

  private void ShowText()
  {
    _label.Text = _lines[0];
    _lines.RemoveAt(0);
  }

  private void NextLine()
  {
    if (_lines.Count == 0)
      Finish();
    else
      ShowText();
  }

  private void Finish()
  {
    Hide();
    _label.Text = "";
    GetTree().Paused = false;
  }
}
