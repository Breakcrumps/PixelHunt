using System;
using Godot;

namespace PixelHunt.Mechanics.Pulse;

internal partial class PulseSource : Node
{
  internal event Action<PulseParams>? Pulse;

  public override void _Ready()
    => AddToGroup("PulseSources");

  private protected void EmitPulse(PulseParams pulseParams)
    => Pulse?.Invoke(pulseParams);
}
