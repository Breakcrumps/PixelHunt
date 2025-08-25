using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Mechanics.Aim;
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

  private GameTime _time;

  private readonly FunctionComposer _homingFunction = new(
    new QuadraticComponent { A = .07f },
    new LinearComponent { A = 0f, Start = 30 },
    new EndComponent { Start = int.MaxValue }
  );

  internal override void Enter()
  {
    if (_aimArea is null)
      return;

    _aimArea.CanRetarget = false;

    _time = GameTime.Zero;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    if (_aimArea is null || _aimArea.Target is null)
      return;

    _time.Frames++;

    Vector3 difVector = _aimArea.Target.GlobalPosition - _playerChar.GlobalPosition;

    if (difVector.IsRoughlyZero(tolerance: 2f))
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      return;
    }

    _playerChar.Velocity = difVector.Normalized() * GetSpeed(_time);
  }

  private float GetSpeed(GameTime time)
    => _homingFunction.Execute(time);

  internal override void Exit()
  {
    if (_aimArea is null)
      return;

    _aimArea.CanRetarget = true;
  }
}
