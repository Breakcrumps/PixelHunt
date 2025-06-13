using Godot;

public partial class Amogus : CharacterBody3D
{
  [Export] private Movement? _movement;
  [Export] private CameraController? _cameraController;
  [Export] private Animator? _animator;

  public override void _PhysicsProcess(double delta)
  {
    _movement!.Move();
    _cameraController!.AlignBody(delta);


    AnimateMovement();
  }

  private void AnimateMovement()
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    if (horizontalVelocity == Vector2.Zero || !IsOnFloor())
      _animator!.CurrentAnim = Anim.Idle;
    else if (Input.IsActionPressed("Run"))
      _animator!.CurrentAnim = Anim.Run;
    else if (_movement!.SlowWalk)
      _animator!.CurrentAnim = Anim.SlowWalk;
    else
      _animator!.CurrentAnim = Anim.Walk;
  }
}
