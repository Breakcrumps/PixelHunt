using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Pulse;

[GlobalClass]
internal abstract partial class PulseObeyer : Node
{
  public override void _Ready()
    => NodeGroups.PulseSources.ForEach(x => x.Pulse += ObeyPulse);

  public override void _ExitTree()
    => NodeGroups.PulseSources.ForEach(x => x.Pulse -= ObeyPulse);

  private protected abstract void ObeyPulse(PulseParams pulseParams);
}
