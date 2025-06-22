using Godot;

[GlobalClass]
public partial class AttackState : State
{
  [Export] private Model? _model;
  [Export] private Enemy? _enemy;
  [Export] private StateMachine? _stateMachine;
  [Export] private Animator? _animator;

  public override void _Ready()
  {
    _model!.AnimationPlayer!.AnimationFinished += Finish;
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