using Godot;

public partial class WriteFlags : Button
{
  public override void _Ready()
  {
    Pressed += DialogueManager.WriteFlags;
  }
}
