using Godot;

public partial class ResetFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.ResetFlags;
  }
}
