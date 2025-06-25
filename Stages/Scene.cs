using Godot;

[GlobalClass]
public partial class Scene : Node3D
{
  public override void _Ready()
  {
    DialogueManager.UpdateDialogueCache(Name);

    if (Flags.Debug)
    {
      DialogueManager.DumpDialogueCache();
      DialogueManager.DumpChoiceCache();
      DialogueManager.DumpFlags();
    }
  }
}
