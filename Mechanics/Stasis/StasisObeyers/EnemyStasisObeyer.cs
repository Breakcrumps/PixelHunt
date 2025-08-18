using Godot;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Enemy.States;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisObeyers;

[GlobalClass]
internal sealed partial class EnemyStasisObeyer : StasisObeyer
{
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private FollowState? _followState;

  private StasisParams? _stasisParams;

  private GameTime _stasisTime = GameTime.Zero;
  internal bool InStasis { get; private set; }

  private protected override void ObeyStasis(StasisParams stasisParams)
  {
    if (_stateMachine is null)
      return;

    if (_stateMachine.CurrentState is not PulseState)
      return;

    InStasis = true;

    _stateMachine.Transition("StopState");
    _stateMachine.CanTransition = false;

    _stasisParams = stasisParams;

    _stasisTime = stasisParams.Duration;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!InStasis)
      return;

    if (_stateMachine is null)
      return;

    if (_followState is null)
      return;

    _stasisTime.Frames--;

    if (_stasisTime == GameTime.Zero)
    {
      InStasis = false;

      _followState.Target = _stasisParams?.Actor;

      _stateMachine.CanTransition = true;
      _stateMachine?.Transition("FollowState");
    }
  }
}
