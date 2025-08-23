using System;
using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Stasis;

[GlobalClass]
internal partial class StasisSource : Node
{
  internal Action<StasisParams>? Stasis;

  public override void _Ready()
    => NodeGroups.StasisSources.Add(this);

  private protected void EmitStasis(StasisParams stasisParams)
    => Stasis?.Invoke(stasisParams);
}
