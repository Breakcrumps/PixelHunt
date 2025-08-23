using System;
using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Pulse;

[GlobalClass]
internal partial class PulseSource : Node
{
  internal event Action<PulseParams>? Pulse;

  public override void _Ready()
    => NodeGroups.PulseSources.Add(this);

  private protected void EmitPulse(PulseParams pulseParams)
    => Pulse?.Invoke(pulseParams);
}
