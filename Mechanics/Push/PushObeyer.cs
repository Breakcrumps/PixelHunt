using Godot;
using PixelHunt.Mechanics.Stasis;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Push;

[GlobalClass]
internal abstract partial class PushObeyer : Node
{
  [Export] private protected StasisObeyer? StasisObeyer { get; private set; }

  [ExportGroup("Parameters")]
  [Export] private protected float _effectStrength = 15f;

  public override void _Ready()
    => NodeGroups.PushSources.ForEach(x => x.Push += ObeyPush);

  private protected abstract void ObeyPush(PushParams pushParams);
}
