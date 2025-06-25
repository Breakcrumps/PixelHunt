using Godot;

[GlobalClass]
public partial class PushbackState : State
{
  [Export] private Enemy? _enemy;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  public Vector3 PushbackDirection { private get; set; }

  private Player? _player;

  public override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    if (_enemy is null || _animHelper is null)
      return;

    _enemy.Velocity = PushbackDirection * _animHelper.Speed;

    _animHelper.AnimationFinished += DisablePushback;
  }

  private void DisablePushback(StringName animName)
  {
    if (_stateMachine is null)
      return;

    if (animName != "Pushback")
      return;

    _animHelper!.AnimationFinished -= DisablePushback;

    _stateMachine.Transition("IdleState");
  }

  public override void PhysicsProcess(double delta)
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