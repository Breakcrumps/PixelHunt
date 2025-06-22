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

    string animation = (
      !IsOnFloor()
      ? Velocity.Y == 0 ? "Hover"
      : Velocity.Y > 0 ? "Rise" : "Fall"
      : horizontalVelocity != Vector2.Zero
      ? horizontalVelocity.Length() > 7f ? "Jog" : "Walk"
      : "Idle"
    );

    double blendTime = (
      animation == "Fall"
      ? .21
      : .15
    );

    _animator?.PlayAnimation(animation, blendTime);
  }

  public override void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Damage;

    if (Flags.Debug)
      GD.Print($"Player was hit for {attack.Damage}HP, {Health}HP left.");

    if (Health <= 0)
      Die();
  }
}
