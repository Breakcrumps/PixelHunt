using Godot;

[GlobalClass]
public partial class VisionCone : Area3D
{
  [Export] private EnemyStateMachine? _stateMachine;

  [Export] private FollowState? _followState;

  [Export] private CollisionShape3D? _vision;

  [Export] private RayCast3D? _rayCast;

  private Player? _player;

  private bool _playerInSight;

  public override void _Ready()
  {
    BodyEntered += StartTracking;
    BodyExited += StopTracking;
  }

  private void StartTracking(Node3D node)
  {
    if (node is not Player player)
      return;

    if (Flags.Debug)
      GD.Print($"{player.Name} entered VisionCone sight!");

    _playerInSight = true;
    _player = player;
  }

  private void StopTracking(Node3D node)
  {
    if (node is not Player player)
      return;

    if (Flags.Debug)
      GD.Print($"{player.Name} exited VisionCone sight!");

    _playerInSight = false;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!_playerInSight)
      return;

    if (_rayCast is null)
      return;

    if (_player is null)
      return;

    _rayCast.TargetPosition = _player.GlobalPosition - _rayCast.GlobalPosition;

    if (_rayCast?.GetCollider() is not Player)
      return;

    _stateMachine?.Transition("FollowState");
    _followState!.Player = _player;
  }

  public void DisableSearch()
  {
    _vision!.Disabled = true;
    _playerInSight = false;
  }

  public void EnableSearch()
  {
    _vision!.Disabled = false;
  }
}
