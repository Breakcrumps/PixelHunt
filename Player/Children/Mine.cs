using Godot;

public partial class Mine : Node3D
{
  [Export] public AnimationPlayer? AnimationPlayer;
  [Export] public AnimationTree? AnimationTree { get; private set; }
}
