using Godot;

namespace PixelHunt.Mechanics.Stasis;

[GlobalClass]
internal abstract partial class StasisObeyer : Node
{
  public override void _Ready()
  {
    foreach (Node node in GetTree().GetNodesInGroup("StasisSources"))
    {
      if (node is not StasisSource stasisSource)
        continue;

      stasisSource.Stasis += ObeyStasis;
    }
  }

  private protected abstract void ObeyStasis(StasisParams stasisParams);
}
