using Godot;

[GlobalClass]
public partial class FollowState : State
{
  [Export] Enemy? _enemy;
  [Export] private Animator? _animator;
  [Export] private StateMachine? _stateMachine;

  [ExportGroup("Parameters")]
  [Export] private float _chaseSpeed = .4f;

  private Player? _player;

  public override void Enter()
  {
    _player = (Player)GetTree().GetFirstNodeInGroup("Player");
  }

  public override void PhysicsProcess(double delta)
  {
    if (_enemy is null)
      return;

    Vector3 diffVector = _player!.GlobalPosition - _enemy.GlobalPosition;
    Vector2 direction = new(diffVector.X, diffVector.Z);

    if (direction.Length() > 10f)
    {
      _stateMachine?.Transition("Idle");
      return;
    }

    if (direction.Length() > 2f)
    {
      Vector2 velocity = direction.Normalized() * _animator!.Mine!.MovementAnimation!.Speed;

      _enemy.Velocity = _enemy.Velocity with
      {
        X = velocity.X,
        Z = velocity.Y
      };

      if (_animator is not null)
      {
        _animator.CurrentAnim = Anim.Walk;
        _animator.PlayMovementAnimation("Walk");
      }
    }
    else
    {
      _enemy.Velocity = _enemy.Velocity with
      {
        X = 0f,
        Z = 0f
      };

      if (_animator is not null)
        _animator.CurrentAnim = Anim.Idle;
    }

    _enemy.AlignBody(delta);
  }
}