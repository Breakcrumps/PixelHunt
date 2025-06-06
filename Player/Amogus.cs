using Godot;

public partial class Amogus : CharacterBody3D
{
  [ExportGroup("Composites")]
  [Export] private Movement _movement;
  [Export] private CameraController _cameraController;

  public bool CanMove { get; set; } = true;

  public override void _PhysicsProcess(double delta)
  {
    if (!CanMove)
      return;
      
    _movement.Move();
    _cameraController.AlignBody(delta);
  }
}
