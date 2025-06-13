using Godot;

public partial class PlayerTree : AnimationTree
{
  public override void _Ready()
  {
    EventBus.BlendParameterChange += (parameter, value)
      => Set(parameter, value);
  }
}
