using Godot;

[GlobalClass]
public partial class AttackState : State
{
  [Export] private Enemy? _enemy;
  [Export] private Animator? _animator;

  public override void Enter()
  {
    _animator?.PlayAnimation("Attack");
    _enemy!.Velocity = Vector3.Zero;
  }
}