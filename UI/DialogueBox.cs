using System.Collections.Generic;
using System.Linq;
using Godot;

[GlobalClass]
internal partial class DialogueBox : Control
{
  private AudioPlayer? _audioPlayer;

  [Export] private RichTextLabel? _nameBox;
  [Export] private RichTextLabel? _textBox;
  [Export] private ChoiceContainer? _choiceContainer;
  [Export] private Timer? _timer;

  [ExportGroup("Parameters")]
  [Export] private float _cps = 25f;

  private List<Replica> _lines = [];
  private List<Replica> _choiceLines = [];
  private Dictionary<string, List<Replica>> _pendingOptions = [];

  private bool _inChoice;
  private bool _waiting;

  private List<int> _waitIndices = [];

  public override void _Ready()
  {
    _audioPlayer = (AudioPlayer)GetTree().GetFirstNodeInGroup("AudioPlayer");

    _timer!.WaitTime = 1f / _cps;
    _timer.Timeout += NextSymbol;

    Hide();
  }

  public override void _Input(InputEvent @event)
  {
    if (!Visible)
      return;

    AcceptEvent();

    if (_inChoice)
      HandleChoice(@event);

    else if (@event.IsActionPressed("Confirm"))
      HandleConfirm();
  }

  private void HandleConfirm()
  {
    if (_waiting)
    {
      _waiting = false;
      _timer?.Start();
    }

    else if (_textBox!.VisibleRatio.IsRoughly(1f))
    {
      if (_waitIndices.Count == 0)
      {
        _textBox.VisibleRatio = 1f;
      }
      else
      {
        _textBox.VisibleRatio = (float)_waitIndices[0] / _textBox.Text.Length;
        _waiting = true;
        _waitIndices.RemoveAt(0);
      }

      _timer?.Stop();
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

    _choiceLines = [.. lineList[optionIndex]];

    _choiceContainer!.HideChoices();

    _inChoice = false;

    ShowLine(_choiceLines);
  }

  internal void InitDialogue(string source)
  {
    _lines = [.. DialogueManager.Dialogue[source]];

    GetTree().Paused = true;

    _nameBox!.Text = source;

    Show();

    ShowLine(_lines);
  }

  private void InitChoice(string label)
  {
    _pendingOptions = new Dictionary<string, List<Replica>>(DialogueManager.Choices[label]);

    List<string> optionSummaries = [.. _pendingOptions.Select(x => x.Key).Where(x => x != "")];

    _choiceContainer!.ShowChoices(optionSummaries);

    _nameBox!.Text = _pendingOptions[""][0].Who;

    _textBox!.VisibleRatio = 1f;
    _textBox.Text = _pendingOptions[""][0].Line;

    _inChoice = true;
  }

  private void ShowLine(List<Replica> lines)
  {
    Replica next = lines[0];
    lines.RemoveAt(0);

    if (Flags.Debug)
      GD.Print(next);

    if (next.Sound is not null)
    {
      _audioPlayer?.PlaySound(next.Sound);
    }

    if (next.Conditions is not null)
    {
      if (next.Conditions.Any(conditionName => !DialogueManager.Flags[conditionName]))
      {
        NextLine();
        return;
      }
    }

    if (next.Actions is not null)
    {
      foreach (string flagName in next.Actions)
      {
        DialogueManager.Flags[flagName] = true;

        DialogueManager.DumpFlags();
      }
    }

    if (next.Choice is not null)
    {
      InitChoice(next.Choice);
      return;
    }

    UpdateWaitIndices(next.Line);

    _nameBox!.Text = next.Who;
    _textBox!.Text = next.RawLine;

    _textBox.VisibleRatio = 0f;

    _timer!.Start();
    NextSymbol();
  }

  private void NextSymbol()
  {
    int index = (int)(_textBox!.Text.Length * _textBox.VisibleRatio);

    if (_waitIndices.Contains(index))
    {
      _waiting = true;
      _timer!.Stop();
      _waitIndices.RemoveAt(0);
      return;
    }

    _textBox.VisibleRatio += 1f / _textBox.Text.Length;

    if (_textBox.VisibleRatio.IsRoughly(1f))
      _timer!.Stop();
  }

  private void NextLine()
  {
    if (_lines.Count == 0)
      Finish();

    else if (_choiceLines.Count == 0)
      ShowLine(_lines);

    else
      ShowLine(_choiceLines);
  }

  private void UpdateWaitIndices(string line)
  {
    _waitIndices = [];

    for (int i = 0; i < line.Length; i++)
    {
      if (line[i] == '|')
      {
        _waitIndices.Add(i);
      }
    }

    if (Flags.Debug)
      GD.Print($"Wait indices: {string.Join(", ", _waitIndices)}");
  }

  private void Finish()
  {
    Hide();
    _textBox!.Text = "";
    _nameBox!.Text = "";
    GetTree().Paused = false;
  }
}
