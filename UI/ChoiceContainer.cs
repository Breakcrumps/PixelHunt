using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ChoiceContainer : Control
{
  private List<Control> optionContainers = [];

  public override void _Ready()
  {
    optionContainers = [.. GetChild(0).GetChildren().Select(x => (Control)x)];

    optionContainers.ForEach(x => x.Visible = false);
  }

  public void ShowChoices(List<string> optionSummaries)
  {
    for (int i = 0; i < optionSummaries.Count; i++)
    {
      optionContainers[i].Visible = true;
      optionContainers[i].GetChild<RichTextLabel>(1).Text = $"{i + 1}: {optionSummaries[i]}";
    }
  }

  public void HideChoices()
  {
    optionContainers.ForEach(x => x.Visible = false);
  }
}
