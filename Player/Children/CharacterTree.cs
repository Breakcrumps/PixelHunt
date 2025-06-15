using Godot;

[GlobalClass]
public partial class CharacterTree : AnimationTree
{
  public override void _Ready()
  {
    EventBus.NotifyCreation(this);
  }
}
