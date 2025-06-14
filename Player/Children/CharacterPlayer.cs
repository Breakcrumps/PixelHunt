using Godot;

public partial class CharacterPlayer : AnimationPlayer
{
  public override void _Ready()
  {
    AnimationBus.AnimPlay += (animName, blendTime) => { Play(animName, blendTime); };
  }
}
