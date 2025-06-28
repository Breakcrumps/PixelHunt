using Godot;

[GlobalClass]
internal partial class AttackState : State
{
  [Export] private Enemy? _enemy;
  [Export] private Animator? _animator;

  internal override void Enter()
  {
    _animator?.PlayAnimation("Attack");
    _enemy!.Velocity = Vector3.Zero;
  }
}