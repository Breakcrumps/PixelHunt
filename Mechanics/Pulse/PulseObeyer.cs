using Godot;

namespace PixelHunt.Mechanics.Pulse;

internal abstract partial class PulseObeyer : Node
{
  public override void _Ready()
  {
    PulseSource pulseSource = (PulseSource)GetTree().GetFirstNodeInGroup("PulseSources");

    pulseSource.Pulse += ObeyPulse;
  }

  private protected abstract void ObeyPulse(PulseParams pulseParams);
}
