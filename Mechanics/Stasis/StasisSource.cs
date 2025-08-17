using System;
using Godot;

namespace PixelHunt.Mechanics.Stasis;

[GlobalClass]
internal partial class StasisSource : Node
{
  internal Action<StasisParams>? Stasis;

  public override void _Ready()
    => AddToGroup("StasisSources");

  private protected void EmitStasis(StasisParams stasisParams)
    => Stasis?.Invoke(stasisParams);
}
