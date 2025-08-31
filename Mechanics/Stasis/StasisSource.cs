using System;
using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Stasis;

[GlobalClass]
internal abstract partial class StasisSource : Node
{
  internal event Action<StasisParams>? Stasis;

  public override void _Ready()
    => NodeGroups.StasisSources.Add(this);

  public override void _ExitTree()
    => NodeGroups.StasisSources.Remove(this);

  private protected void EmitStasis(StasisParams stasisParams)
    => Stasis?.Invoke(stasisParams);
}
