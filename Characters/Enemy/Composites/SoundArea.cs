using Godot;
using GameSrc.Enemy.States;
using GameSrc.Player;
using GameSrc.Static;

namespace GameSrc.Enemy.Composites;

[GlobalClass]
internal sealed partial class SoundArea : Area3D
{
  [Export] private EnemyStateMachine? _enemyStateMachine;

  [Export] private FollowState? _followState;

  [Export] private CollisionShape3D? _soundCollision;

  public override void _Ready()
  {
    BodyEntered += NoticePlayer;
  }

  private void NoticePlayer(Node3D node)
  {
    if (node is not PlayerChar playerChar)
      return;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"{playerChar.Name} entered VisionCone sight!");

    _enemyStateMachine?.Transition("FollowState");
    _followState!.PlayerChar = playerChar;
  }

  internal void DisableSearch()
  {
    _soundCollision!.Disabled = true;
  }

  internal void EnableSearch()
  {
    _soundCollision!.Disabled = false;
  }
}
