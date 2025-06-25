using Godot;

[GlobalClass]
public partial class AttackState : State
{
  [Export] private AnimationPlayer? _animPlayer;
  [Export] private Enemy? _enemy;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private Animator? _animator;

  public override void _Ready()
  {
    _animPlayer!.AnimationFinished += Finish;
  }

  public override void Enter()
  {
    _animator?.PlayAnimation("Attack");
    _enemy!.Velocity = Vector3.Zero;
  }

  private void Finish(StringName animName)
  {
    if (animName != "Attack")
      return;

    _stateMachine!.Transition("FollowState");
  }
}