using Godot;

public partial class Mine : Node3D
{
  [Export] public AnimationPlayer? AnimationPlayer { get; private set; }
  [Export] public AnimationTree? AnimationTree { get; private set; }
  [Export] public MovementAnimation? MovementAnimation { get; private set; }
}
