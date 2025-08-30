using Godot;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Mechanics.Pulse.PulseObeyers;
using PixelHunt.Mechanics.Stasis.StasisObeyers;
using PixelHunt.World;

[GlobalClass]
internal sealed partial class WallRunArea : Area3D
{
  [Export] private LargeRubbish? _wall;
  [Export] private RigidPulseObeyer? _pulseObeyer;
  [Export] private RigidStasisObeyer? _stasisObeyer;

  public override void _Ready()
    => BodyEntered += HandleWallRunTransition;

  private void HandleWallRunTransition(Node3D node)
  {
    if (
      (_pulseObeyer is null || !_pulseObeyer.Pulsing)
      && (_stasisObeyer is null || !_stasisObeyer.InStasis)
    )
      return;

    if (node is not PlayerChar playerChar)
      return;

    if (playerChar.IsOnFloor())
      return;

    MoveStateMachine? moveStateMachine = playerChar.MoveStateMachine;

    if (moveStateMachine is null || moveStateMachine.WallRunStrategy is null)
      return;

    moveStateMachine.WallRunStrategy.WallRunArea = this;
    moveStateMachine.Transition("WallRunStrategy");
  }
}
