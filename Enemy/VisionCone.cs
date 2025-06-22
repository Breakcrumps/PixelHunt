using Godot;

public partial class VisionCone : Area3D
{
  [Export] private StateMachine? _stateMachine;

  [Export] private FollowState? _followState;

  [Export] private CollisionShape3D? _vision;
  [Export] private CollisionShape3D? _sound;

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

    DisableSearch();
  }

  private void DisableSearch()
  {
    _vision!.Disabled = true;
    _sound!.Disabled = true;
  }

  public void EnableSearch()
  {
    _vision!.Disabled = false;
    _sound!.Disabled = false;
  }
}
