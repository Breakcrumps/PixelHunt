using Godot;
using PixelHunt.Characters.Player.CameraModes;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.BodyAligns;

[GlobalClass]
internal sealed partial class DefaultAlign : State
{
  [Export] private BodyAlignStateMachine? _bodyAligner;

  [Export] private PlayerChar? _playerChar;
  [Export] private Skeleton3D? _armature;
  [Export] private CameraStateMachine? _cameraStateMachine;

  [ExportGroup("Parameters")]
  [Export] private float _turnSpeed = 10f;

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;
      
    if (_cameraStateMachine is null)
      return;

    if (!_playerChar.IsOnFloor() && _cameraStateMachine.CurrentState is LockOnCameraMode)
    {
      _bodyAligner?.Transition("LockOnAlign");
      return;
    }

    AlignBody(delta);
  }

  private void AlignBody(double delta)
  {
    if (_armature is null)
      return;

    Vector2 horizontalVelocity = new(_playerChar!.Velocity.X, _playerChar.Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _armature.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: horizontalVelocity.AngleTo(Vector2.Down),
        weight: _turnSpeed * (float)delta
      )
    };
  }
}
