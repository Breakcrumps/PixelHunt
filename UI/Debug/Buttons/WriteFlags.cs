using GameSrc.Dialogue;
using Godot;

internal partial class WriteFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.WriteFlags;
  }
}
