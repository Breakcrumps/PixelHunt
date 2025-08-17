using Godot;

namespace PixelHunt.Mechanics.Pulse;

[GlobalClass]
internal abstract partial class PulseObeyer : Node
{
  public sealed override void _Ready()
  {
    foreach (Node node in GetTree().GetNodesInGroup("PulseSources"))
    {
      if (node is not PulseSource pulseSource)
        continue;

      pulseSource.Pulse += ObeyPulse;
    }
  }

  private protected abstract void ObeyPulse(PulseParams pulseParams);
}
