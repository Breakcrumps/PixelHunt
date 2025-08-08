using PixelHunt.Dialogue;
using PixelHunt.Static;
using Godot;

namespace PixelHunt.Stages;

[GlobalClass]
internal sealed partial class Scene : Node3D
{
  public override void _Ready()
  {
    DialogueManager.UpdateDialogueCache(Name);

    if (DebugFlags.GetDebugFlag(this))
    {
      DialogueManager.DumpDialogueCache();
      DialogueManager.DumpChoiceCache();
      DialogueManager.DumpFlags();
    }
  }
}
