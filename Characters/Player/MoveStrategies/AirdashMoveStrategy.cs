using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Nullifiers;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class AirdashMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private CameraPivot? _cameraPivot;

  [Export] private MoveStateMachine? _moveStateMachine;

  [Export] private bool _verticalDash;

  internal Vector3 DashDirection { private get; set; }

  private GameTime _dashTime;

  private static readonly FunctionComposer _dashSpeedComposer = new(
    new LinearNullifier { StartValue = 55f, End = 50 },
    new EndComponent { Start = 50 }
  );

  internal override void Enter()
  {
    if (_cameraPivot is null)
      return;
    
    DashDirection = Input.GetVector(
      negativeX: "Left", positiveX: "Right",
      negativeY: "Up", positiveY: "Down"
    ).ToVector3();
    DashDirection = DashDirection.Rotated(Vector3.Up, _cameraPivot.Rotation.Y);

    if (!_verticalDash)
      DashDirection = DashDirection with { Y = 0f };

    _dashTime = GameTime.Zero;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    if (_dashTime == GameTime.Frame * 13)
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      return;
    }

    if (_playerChar.IsOnFloor())
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      return;
    }

    float speed = _dashSpeedComposer.Execute(_dashTime);
    _playerChar.Velocity = DashDirection.Normalized() * speed;

    _dashTime.Frames++;
  }
}
