using Godot;

public partial class Player : Character
{
  [Export] private Movement? _movement;
  [Export] private CameraController? _cameraController;
  [Export] private Animator? _animator;

  public override void _PhysicsProcess(double delta)
  {
    _movement!.Move(delta);

    AnimateMovement();
  }

  private void AnimateMovement()
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    _animator!.MovingSpeed = horizontalVelocity.Length();

    _animator!.CurrentAnim = (
      !IsOnFloor()
      ? Velocity.Y == 0 ? Anim.Hover
      : Velocity.Y > 0 ? Anim.Rise : Anim.Fall
      : horizontalVelocity != Vector2.Zero
      ? horizontalVelocity.Length() > 7f ? Anim.Jog : Anim.Walk
      : Anim.Idle
    );
  }
}
