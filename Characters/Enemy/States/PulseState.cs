using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Animation;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Mechanics.Pulse;
using PixelHunt.Parents;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class PulseState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _enemyStateMachine;
  [Export] private FollowState? _followState;
  [Export] private GravityComposite? _gravityComposite;
  [Export] private Animator? _animator;

  internal PulseParams? PulseParams { private get; set; }
  internal FunctionComposer PulseFunction { private get; set; } = PulseFunctions.GenerateEnemyFunction(level: 1);

  private float _initialHeight;

  private GameTime _currentTime;

  internal override void Enter()
  {
    if (PulseParams is not null)
      PulseFunction = PulseFunctions.GenerateEnemyFunction(PulseParams.Level);

    _currentTime = GameTime.Zero;

    _animator?.PlayAnimation("Idle");

    if (_enemyChar is not null)
    {
      _initialHeight = _enemyChar.GlobalPosition.Y;
      _enemyChar.Velocity = Vector3.Zero;
    }
    
    if (_gravityComposite is not null)
      _gravityComposite.CanGravitate = false;
  }
  
  internal override void Exit()
  {
    if (_gravityComposite is null)
      return;
    
    _gravityComposite.CanGravitate = true;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (PulseParams is null)
      return;

    if (_enemyChar is null)
      return;

    if (_followState is null)
      return;

    _currentTime.Frames += 1;

    _enemyChar.GlobalPosition = _enemyChar.GlobalPosition with
    {
      Y = _initialHeight + PulseFunction.Execute(_currentTime.Frames)
    };

    if (_currentTime == PulseFunction.ResultDuration)
    {
      _followState.Target = PulseParams.Actor;
      _enemyStateMachine?.Transition("FollowState");
    }
  }
}
