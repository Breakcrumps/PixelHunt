using Godot;
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

  internal PulseParams? PulseParams { private get; set; }

  private float _initialHeight;

  private GameTime _currentTime;

  internal override void Enter()
  {
    _currentTime = new GameTime(0);
    
    if (_enemyChar is not null)
      _initialHeight = _enemyChar.GlobalPosition.Y;
    
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
      Y = _initialHeight + PulseParams.PositionComputer.Execute(_currentTime.Frames)
    };

    if (_currentTime == PulseParams.Duration)
    {
      if (DebugFlags.GetDebugFlag(this))
        GD.Print("It ended.");

      _followState.Target = PulseParams.Actor;
      _enemyStateMachine?.Transition("FollowState");
    }
  }
}
