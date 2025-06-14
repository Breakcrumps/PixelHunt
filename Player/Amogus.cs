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
    if (Flags.FunFlightShenanigans)
    {
      _animator!.CurrentAnim = (
        Velocity == Vector3.Zero
        ? Anim.Hover
        : Anim.Rise
      );

      return;
    }

    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    _animator!.CurrentAnim = (
      !IsOnFloor()
      ? Velocity.Y == 0 ? Anim.Hover
      : Velocity.Y > 0 ? Anim.Rise : Anim.Fall
      : horizontalVelocity != Vector2.Zero
      ? horizontalVelocity.Length() > 15f ? Anim.Run : Anim.Walk
      : Anim.Idle
    );
  }
}
