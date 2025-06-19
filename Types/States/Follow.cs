using Godot;

[GlobalClass]
public partial class Follow : State
{
  [Export] Enemy? _enemy;
  [Export] private StateMachine? _stateMachine;

  [ExportGroup("Parameters")]
  [Export] private float _chaseSpeed = .3f;

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
      Vector2 velocity = direction.Normalized() * _chaseSpeed;

      _enemy.Velocity = _enemy.Velocity with
      {
        X = velocity.X,
        Z = velocity.Y
      };
    }
    else
    {
      _enemy.Velocity = _enemy.Velocity with
      {
        X = 0f,
        Z = 0f
      };
    }

    _enemy.AlignBody(delta);
  }
}