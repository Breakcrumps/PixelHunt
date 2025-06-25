using System.Collections.Generic;
using System.Linq;
using Godot;

[GlobalClass]
public partial class ChoiceContainer : Control
{
  private List<Control> _optionContainers = [];

  public override void _Ready()
  {
    _optionContainers = [.. GetChild(0).GetChildren().Select(x => (Control)x)];

    _optionContainers.ForEach(x => x.Visible = false);
  }

  public void ShowChoices(List<string> optionSummaries)
  {
    for (int i = 0; i < optionSummaries.Count; i++)
    {
      _optionContainers[i].Visible = true;
      _optionContainers[i].GetChild<RichTextLabel>(1).Text = $"{i + 1}: {optionSummaries[i]}";
    }
  }

  public void HideChoices()
  {
    _optionContainers.ForEach(x => x.Visible = false);
  }
}
