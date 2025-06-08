using Godot;

public partial class LoadFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.LoadFlags;
  }
}
