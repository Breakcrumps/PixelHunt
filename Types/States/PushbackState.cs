using Godot;

[GlobalClass]
public partial class PushbackState : State
{
  [Export] private Enemy? _enemy;
  [Export] private StateMachine? _stateMachine;

  private Player? _player;

  public override void PhysicsProcess(double delta)
  {
    if (_enemy is null)
      return;

    _enemy.Velocity *= .9f;

    _enemy.AlignBody(delta, inverse: true);

    if (Mathf.Abs(_enemy.Velocity.X) < .01f && Mathf.Abs(_enemy.Velocity.Z) < .01f)
      _stateMachine?.Transition("Idle");
  }
}