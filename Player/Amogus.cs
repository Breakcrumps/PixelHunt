using Godot;

public partial class Amogus : CharacterBody3D
{
  [ExportGroup("Composites")]
  [Export] private Movement _movement;
  [Export] private CameraController _cameraController;

  public override void _PhysicsProcess(double delta)
  {
    _movement.Move();
    _cameraController.AlignBody(delta);
  }
}
