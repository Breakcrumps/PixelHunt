using Godot;

[GlobalClass]
internal partial class FollowState : State
{
  [Export] Enemy? _enemy;
  [Export] private Animator? _animator;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private VisionCone? _visionArea;
  [Export] private SoundArea? _soundArea;
  [Export] private AnimationHelper? _animHelper;

  internal Player? Player { private get; set; }

  internal override void Enter()
  {
    _visionArea?.DisableSearch();
    _soundArea?.DisableSearch();

    Player player = (Player)GetTree().GetFirstNodeInGroup("Player");

    player.Unsheathe();
  }

  internal override void PhysicsProcess(double delta)
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

    if (direction.Length() > 30f)
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