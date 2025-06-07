using System.Collections.Generic;
using Godot;

public partial class DialogueBox : CanvasLayer
{
  [Export] private Label _label;
  [Export] private Timer _timer;

  [Export] private float _cps = 25f;

  private List<string> _lines = [];

  public override void _Ready()
  {
    _timer.WaitTime = 1f / _cps;
    _timer.Timeout += NextSymbol;

    Hide();

    EventBus.DialogueInit += InitDialogue;
  }

  public override void _Input(InputEvent @event)
  {
    if (!Visible)
      return;

    if (!@event.IsActionPressed("Confirm"))
      return;

    GetViewport().SetInputAsHandled();

    if (_label.VisibleRatio != 1f)
    {
      _label.VisibleRatio = 1f;
      _timer.Stop();
    }
    else
    {
      NextLine();
    }
  }

  private void InitDialogue(string source)
  {
    _lines = [.. DialogueManager.Dialogue[source]];

    GetTree().Paused = true;

    Show();

    ShowText();
  }

  private void ShowText()
  {
    _label.Text = _lines[0];
    _lines.RemoveAt(0);

    _label.VisibleRatio = 0f;

    _timer.Start();
    NextSymbol();
  }

  private void NextSymbol()
  {
    _label.VisibleRatio += 1f / _label.Text.Length;
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
