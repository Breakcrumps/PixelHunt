using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.Functions;
using PixelHunt.Characters.Enemy;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Enemy.States;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisObeyers;

[GlobalClass]
internal sealed partial class EnemyStasisObeyer : StasisObeyer
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private FollowState? _followState;

  private StasisParams? _stasisParams;
  private FunctionComposer? _stasisWiggle;
  private int _wiggleAxis;

  private GameTime _stasisTime;

  private float _initialWigglePos;

  private protected override void ObeyStasis(StasisParams stasisParams)
  {
    if (_enemyChar is null)
      return;

    if (_stateMachine is null)
      return;

    if (_stateMachine.CurrentState is not PulseState)
      return;

    InStasis = true;

    _stateMachine.Transition("StopState");
    _stateMachine.CanTransition = false;

    _stasisParams = stasisParams;

    _stasisTime = GameTime.Zero;

    _stasisWiggle = StasisFunctions.GenerateStasisWiggle();
    _wiggleAxis = StasisFunctions.WiggleAxes.RandomItem();

    _initialWigglePos = _enemyChar.GlobalPosition[_wiggleAxis];
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!InStasis)
      return;

    if (_stateMachine is null)
      return;

    if (_followState is null)
      return;

    if (_enemyChar is null)
      return;

    if (_stasisParams is null)
      return;

    if (_stasisWiggle is null)
      return;

    _stasisTime.Frames++;

    Vector3 newGlobalPosition = _enemyChar.GlobalPosition;
    newGlobalPosition[_wiggleAxis] = _initialWigglePos + _stasisWiggle.ExecuteOrZero(_stasisTime);
    _enemyChar.GlobalPosition = newGlobalPosition;

    if (_stasisTime == _stasisParams.Duration)
    {
      InStasis = false;

      _stateMachine.CanTransition = true;

      _followState.Target = _stasisParams.Actor;
      _stateMachine?.Transition("FollowState");
    }
  }
}
