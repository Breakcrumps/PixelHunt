using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DialogueBox : CanvasLayer
{
  [Export] private RichTextLabel _nameBox;
  [Export] private RichTextLabel _textBox;
  [Export] private Timer _timer;

  [Export] private float _cps = 25f;

  private List<Replica> _lines = [];
  private Dictionary<string, List<Replica>> _pendingOptions = [];

  private bool _inChoice;

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

    GetViewport().SetInputAsHandled();

    if (_inChoice)
    {
      HandleChoice(@event);
    }

    else if (@event.IsActionPressed("Confirm"))
    {
      HandleConfirm();
    }
  }

  private void HandleConfirm()
  {
    if (_textBox.VisibleRatio != 1f)
    {
      _textBox.VisibleRatio = 1f;
      _timer.Stop();
    }

    else
    {
      NextLine();
    }
  }

  private void HandleChoice(InputEvent @event)
  {
    int optionIndex = (
      @event.IsActionPressed("First") ? 1
      : @event.IsActionPressed("Second") ? 2
      : @event.IsActionPressed("Third") ? 3
      : @event.IsActionPressed("Fourth") ? 4
      : -1
    );

    if (optionIndex == -1)
      return;

    List<List<Replica>> lineList = [.. _pendingOptions.Select(x => x.Value)];

    if (optionIndex >= lineList.Count)
      return;

    List<Replica> nextLines = [.. lineList[optionIndex]];

    _inChoice = false;

    ShowText(nextLines);
  }

  private void InitDialogue(string source)
  {
    _lines = [.. DialogueManager.Dialogue[source]];

    GetTree().Paused = true;

    _nameBox.Text = source;

    Show();

    ShowText();
  }

  private void InitChoice(string label)
  {
    _pendingOptions = new(DialogueManager.Choices[label]);

    _nameBox.Text = _pendingOptions[""][0].Who;

    _textBox.VisibleRatio = 1f;
    _textBox.Text = _pendingOptions[""][0].Line;

    _inChoice = true;
  }

  private void ShowText()
  {
    Replica next = _lines[0];
    _lines.RemoveAt(0);

    if (next.Line.StartsWith(':'))
    {
      InitChoice(next.Line);
      return;
    }

    _nameBox.Text = next.Who;
    _textBox.Text = next.Line;

    _textBox.VisibleRatio = 0f;

    _timer.Start();
    NextSymbol();
  }

  private void ShowText(List<Replica> lines)
  {
    Replica next = lines[0];
    lines.RemoveAt(0);

    _nameBox.Text = next.Who;
    _textBox.Text = next.Line;

    _textBox.VisibleRatio = 0f;

    _timer.Start();
    NextSymbol();
  }

  private void NextSymbol()
  {
    _textBox.VisibleRatio += 1f / _textBox.Text.Length;

    if (_textBox.VisibleRatio == 1f)
      _timer.Stop();
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
    _textBox.Text = "";
    GetTree().Paused = false;
  }
}
