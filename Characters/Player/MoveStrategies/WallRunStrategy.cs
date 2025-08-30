using Godot;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;
using PixelHunt.Static;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class WallRunStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private CameraPivot? _cameraPivot;
  [Export] private MoveStateMachine? _moveStateMachine;

  [Export] private float _wallRunSpeed = 10f;

  internal WallRunArea? WallRunArea { private get; set; }

  private Vector3 _direction;

  // internal override bool Condition()
  // {
  //   if (Wall is null)
  //     return false;

  //   if (_playerChar is null)
  //     return false;

  //   float angle = _playerChar.Velocity.AngleTo(Wall.Basis.Z);

  //   return !angle.BetweenRadians(-1f / 4f, 1f / 4f);
  // }

  //!!! DANGEROUS BEHAVIOUR TRYING TO TRANSITION FROM AN ENTER FUNCTION CALLED IN A TRANSITION !!!
  internal override void Enter()
  {
    if (WallRunArea is null)
      return;

    _direction = WallRunArea.GlobalBasis.X;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is not null)
      _playerChar.Velocity = _direction * _wallRunSpeed;

    if (_cameraPivot is not null)
    {
      float targetAngle = new Vector2(_direction.X, _direction.Z).AngleTo(Vector2.Up);

      _cameraPivot.Rotation = _cameraPivot.Rotation with
      {
        Y = _cameraPivot.Rotation.Y.LerpAngleF(to: targetAngle, weight: 5f * (float)delta)
      };
    }
  }
}
