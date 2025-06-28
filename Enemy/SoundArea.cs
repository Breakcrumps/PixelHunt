using Godot;

[GlobalClass]
internal partial class SoundArea : Area3D
{
  [Export] private StateMachine? _stateMachine;

  [Export] private FollowState? _followState;

  [Export] private CollisionShape3D? _soundCollision;

  public override void _Ready()
  {
    BodyEntered += NoticePlayer;
  }

  private void NoticePlayer(Node3D node)
  {
    if (node is not Player player)
      return;

    if (Flags.Debug)
      GD.Print($"{player.Name} entered VisionCone sight!");

    _stateMachine?.Transition("FollowState");
    _followState!.Player = player;
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
