using Godot;

[GlobalClass]
internal partial class AnimationHelper : AnimationPlayer
{
  [Export] internal float Speed { get; set; }

  public override void _Ready()
  {
    Speed = 10f;
  }
}
