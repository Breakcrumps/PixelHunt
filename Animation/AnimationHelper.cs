using Godot;

namespace PixelHunt.Animation;

[GlobalClass]
internal sealed partial class AnimationHelper : AnimationPlayer
{
  [Export] internal float Speed { get; private set; }

  public override void _Ready()
  {
    Speed = 10f;
  }
}
