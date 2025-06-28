using Godot;

[GlobalClass]
internal partial class PushbackState : State
{
  [Export] private Enemy? _enemy;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  internal Vector3 PushbackDirection { private get; set; }

  private Player? _player;

  internal override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    if (_enemy is null || _animHelper is null)
      return;

    _enemy.Velocity = PushbackDirection * _animHelper.Speed;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_enemy is null)
      return;

    if (_animHelper is null)
      _enemy.Velocity *= .9f;
    else
      _enemy.Velocity = PushbackDirection * _animHelper.Speed;

    _enemy.AlignBody(delta, inverse: true);
  }
}