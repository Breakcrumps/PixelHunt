using GameSrc.Static;
using Godot;

internal partial class ResetFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.ResetFlags;
  }
}
