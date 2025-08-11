using Godot;
using PixelHunt.Animation;
using PixelHunt.Characters.Enemy;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.CameraModes;

[GlobalClass]
internal sealed partial class LockOnCameraMode : State
{
  [Export] private CameraStateMachine? _cameraStateMachine;
  [Export] private Node3D? _cameraPivot;
  [Export] private Area3D? _eyesight;
  [Export] private PlayerAnimator? _playerAnimator;

  [ExportGroup("Parameters")]
  [Export] private float _lockOnSpeed = 20f;

  private Character? _targetChar;

  internal override void Enter()
  {
    _playerAnimator?.Unsheathe();

    if (_eyesight is null)
      return;
    if (_cameraPivot is null)
      return;

    foreach (Node node in _eyesight.GetOverlappingBodies())
    {
      if (node is not EnemyChar enemyChar)
        continue;

      if (_targetChar is null)
      {
        _targetChar = enemyChar;
        continue;
      }

      Vector3 currentDistance = _targetChar.GlobalPosition - _cameraPivot.GlobalPosition;
      Vector3 candidateDistance = enemyChar.GlobalPosition - _cameraPivot.GlobalPosition;

      if (candidateDistance.Length() < currentDistance.Length())
      {
        _targetChar = enemyChar;
      }
    }
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_targetChar is null)
      return;
    if (_cameraPivot is null)
      return;

    Quaternion currentRotation = _cameraPivot.Transform.Basis.GetRotationQuaternion();

    Basis targetBasis = Basis.LookingAt(_targetChar.GlobalPosition - _cameraPivot.GlobalPosition);
    Quaternion targetRotation = targetBasis.GetRotationQuaternion();

    _cameraPivot.Transform = _cameraPivot.Transform with
    {
      Basis = new Basis(currentRotation.Slerp(to: targetRotation, weight: _lockOnSpeed * (float)delta))
    };
  }

  internal override void UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionReleased("Run"))
      return;

    _targetChar = null;
    _cameraStateMachine?.Transition("FreeCameraMode");
  }
}
