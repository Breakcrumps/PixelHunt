using Godot;

public partial class Model : Node3D
{
  [Export] public AnimationPlayer? AnimationPlayer { get; private set; }
  [Export] public AnimationHelper? AnimationHelper { get; private set; }
  [Export] public MeshInstance3D? Mesh { get; private set; }
}
