using PixelHunt.Dialogue;
using Godot;

internal partial class LoadFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.LoadFlags;
  }
}
