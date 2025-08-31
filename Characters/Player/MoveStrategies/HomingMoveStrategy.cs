using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Mechanics.Aim;
using PixelHunt.Mechanics.Markers;
using PixelHunt.Parents;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class HomingMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private AimArea? _aimArea;
  [Export] private MoveStateMachine? _moveStateMachine;

  private Marker? _target;

  private GameTime _time;

  private readonly FunctionComposer _speedComposer = new(
    new QuadraticComponent { A = .07f },
    new LinearComponent { A = 0f, Start = 30 }
  );

  internal override void Enter()
  {
    if (_aimArea is null)
      return;

    _aimArea.CanRetarget = false;
    _target = _aimArea.Target?.AimMarker;

    _time = GameTime.Zero;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    if (_target is null)
      return;

    _time.Frames++;

    Vector3 difVector = _target.GlobalPosition - _playerChar.GlobalPosition;

    if (difVector.IsRoughlyZero(tolerance: 2f))
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      return;
    }

    _playerChar.Velocity = difVector.Normalized() * GetSpeed(_time);
  }

  private float GetSpeed(GameTime time)
    => _speedComposer.Execute(time);

  internal override void Exit()
  {
    if (_aimArea is null)
      return;

    _aimArea.CanRetarget = true;

    _target = null;
  }
}
