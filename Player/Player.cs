using Godot;

[GlobalClass]
public partial class Player : Character
{
  [Export] private CameraController? _cameraController;
  [Export] private Animator? _animator;
  [Export] private MoveStateMachine? _moveStateMachine;

  public override void _PhysicsProcess(double delta)
  {
    _moveStateMachine?.PhysicsProcess(delta);

    MoveAndSlide();
  }

  public override void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Damage;

    if (Flags.Debug)
      GD.Print($"Player was hit for {attack.Damage}HP, {Health}HP left.");

    _moveStateMachine?.HandlePushback(attackerPos);

    if (Health <= 0)
      Die();
  }

  public void Unsheathe()
  {
    _animator?.Unsheathe();

    if (_animator is not null)
      _animator.AnimPrefix = "Unsheathed";
  }
}
