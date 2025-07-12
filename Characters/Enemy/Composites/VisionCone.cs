using Godot;
using GameSrc.Enemy.States;
using GameSrc.Player;
using GameSrc.Static;

namespace GameSrc.Enemy.Composites;

[GlobalClass]
internal sealed partial class VisionCone : Area3D
{
  [Export] private bool _canSee = true;

  [Export] private EnemyStateMachine? _enemyStateMachine;
  [Export] private FollowState? _followState;
  [Export] private CollisionShape3D? _vision;
  [Export] private RayCast3D? _rayCast;

  private PlayerChar? _playerChar;

  private bool _playerInSight;

  public override void _Ready()
  {
    BodyEntered += StartTracking;
    BodyExited += StopTracking;
  }

  private void StartTracking(Node3D node)
  {
    if (node is not PlayerChar playerChar)
      return;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"{playerChar.Name} entered VisionCone sight!");

    _playerInSight = true;
    _playerChar = playerChar;
  }

  private void StopTracking(Node3D node)
  {
    if (node is not PlayerChar playerChar)
      return;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"{playerChar.Name} exited VisionCone sight!");

    _playerInSight = false;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!_canSee)
      return;

    if (!_playerInSight)
      return;

    if (_rayCast is null)
      return;

    if (_playerChar is null)
      return;

    _rayCast.TargetPosition = _playerChar.GlobalPosition - _rayCast.GlobalPosition;

    if (_rayCast?.GetCollider() is not PlayerChar)
      return;

    _enemyStateMachine?.Transition("FollowState");
    _followState!.PlayerChar = _playerChar;
  }

  internal void DisableSearch()
  {
    _vision!.Disabled = true;
    _playerInSight = false;
  }

  internal void EnableSearch()
  {
    _vision!.Disabled = false;
  }
}
