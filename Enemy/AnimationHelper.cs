using Godot;

[GlobalClass]
public partial class AnimationHelper : AnimationPlayer
{
  [Export] public float Speed { get; set; }

  public override void _Ready()
  {
    Speed = 10f;
  }
}
