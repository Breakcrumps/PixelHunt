using System;
using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Push;

[GlobalClass]
internal abstract partial class PushSource : Node
{
  internal event Action<PushParams>? Push;
  
  public override void _Ready()
    => NodeGroups.PushSources.Add(this);

  public override void _ExitTree()
    => NodeGroups.PushSources.Remove(this);

  private protected void EmitPush(PushParams pushParams)
    => Push?.Invoke(pushParams);
} 
