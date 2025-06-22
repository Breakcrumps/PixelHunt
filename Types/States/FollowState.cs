using Godot;

[GlobalClass]
public partial class FollowState : State
{
  [Export] Enemy? _enemy;
  [Export] private Animator? _animator;
  [Export] private StateMachine? _stateMachine;
  [Export] private VisionCone? _visionArea;
  [Export] private AnimationHelper? _animHelper;

  [ExportGroup("Parameters")]
  [Export] private float _chaseSpeed = .4f;

  public Player? Player { private get; set; }

  public override void PhysicsProcess(double delta)
  {
    if (_enemy is null)
      return;

    Vector3 diffVector = Player!.GlobalPosition - _enemy.GlobalPosition;
    Vector2 direction = new(diffVector.X, diffVector.Z);

    if (direction.Length() < 2f)
    {
      _stateMachine?.Transition("AttackState");
      return;
    }

    if (direction.Length() > 20f)
    {
      _stateMachine?.Transition("IdleState");
      _visionArea?.EnableSearch();
      return;
    }

    Vector2 velocity = direction.Normalized() * _animHelper!.Speed;

    _enemy.Velocity = _enemy.Velocity with
    {
      X = velocity.X,
      Z = velocity.Y
    };

    _animator?.PlayAnimation("Walk");

    _enemy.AlignBody(delta);
  }
}