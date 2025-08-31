using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Stasis;

[GlobalClass]
internal abstract partial class StasisObeyer : Node
{
  internal bool InStasis { get; private protected set; }
  
  public override void _Ready()
    => NodeGroups.StasisSources.ForEach(x => x.Stasis += ObeyStasis);

  public override void _ExitTree()
    => NodeGroups.StasisSources.ForEach(x => x.Stasis -= ObeyStasis);

  private protected abstract void ObeyStasis(StasisParams stasisParams);
}
