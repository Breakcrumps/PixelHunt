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
internal sealed partial class WallRunStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private CameraPivot? _cameraPivot;
  [Export] private FreeMoveStrategy? _freeMoveStrategy;
  [Export] private MoveStateMachine? _moveStateMachine;

  [Export] private float _wallRunSpeed = 6f;

  private float _initialY;

  internal WallRunArea? WallRunArea { private get; set; }

  private GameTime _timeWallRunning;

  private Vector3 _direction;

  private readonly FunctionComposer _heightFunction = new(
    new SineNullifier { End = 60 },
    new LinearComponent { A = -.5f, Start = 60 }
  );

  public override void _Ready()
    => _heightFunction.Plot("WallRun", upTo: GameTime.Second * 3);

  // internal override bool Condition()
  // {
  //   if (_playerChar is null)
  //     return false;

  //   return _playerChar.Velocity.Y >= -10f;
  // }

  internal override void Enter()
  {
    if (WallRunArea is null)
      return;

    if (_playerChar is null)
      return;

    Vector3 xDir = WallRunArea.GlobalBasis.X;

    _direction = _playerChar.Velocity.AngleTo(xDir) <= Mathf.Pi / 2f ? xDir : -xDir;

    _timeWallRunning = GameTime.Zero;

    _initialY = _playerChar.GlobalPosition.Y;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (WallRunArea is null)
      return;

    _timeWallRunning.Frames++;

    HandleVelocity();

    HandleCameraRotation(delta);

    if (!WallRunArea.GetOverlappingBodies().Contains(_playerChar))
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      return;
    }

    if (Input.IsActionJustPressed("Jump"))
    {
      _moveStateMachine?.Transition("FreeMoveStrategy");
      _freeMoveStrategy?.Jump();
      return;
    }
  }

  private void HandleVelocity()
  {
    if (_playerChar is null)
      return;

    _playerChar.Velocity = _direction.Normalized() * _wallRunSpeed;

    _playerChar.GlobalPosition = _playerChar.GlobalPosition with
    {
      Y = _initialY + _heightFunction.Execute(_timeWallRunning)
    };
  }

  private void HandleCameraRotation(double delta)
  {
    if (_cameraPivot is null)
      return;

    float targetAngle = new Vector2(_direction.X, _direction.Z).AngleTo(Vector2.Up);

    _cameraPivot.Rotation = _cameraPivot.Rotation with
    {
      Y = _cameraPivot.Rotation.Y.LerpAngleF(to: targetAngle, weight: 5f * (float)delta)
    };
  }
}
