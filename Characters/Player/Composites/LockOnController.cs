using Godot;
using PixelHunt.Mechanics.Markers;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.Composites;

// I hate this one more than my life.
[GlobalClass]
internal sealed partial class LockOnController : Node
{
  [Export] private Area3D? _eyesight;
  [Export] private CameraPivot? _cameraPivot;
  [Export] private SpringArm3D? _cameraSpring;
  [Export] private PlayerChar? _playerChar;
  [Export] private Camera3D? _camera;

  private Vector3 _initialPivotPosition;
  private float _initialSpringLength;
  private float _initialFov;

  private GameTime _timeFacingTheWrongWay = GameTime.Zero; // Genius name.
  private bool _overflow;

  private ILockOnMarkerBearer? _target;

  public override void _Ready()
  {
    if (_cameraPivot is not null)
      _initialPivotPosition = _cameraPivot.Position;

    if (_cameraSpring is not null)
      _initialSpringLength = _cameraSpring.SpringLength;

    if (_camera is not null)
      _initialFov = _camera.Fov;
  }


  public override void _PhysicsProcess(double delta)
  {
    if (_cameraPivot is null)
      return;

    if (_cameraSpring is null)
      return;

    if (_camera is null)
      return;

    _overflow = _timeFacingTheWrongWay == GameTime.Second;

    HandleInput(delta);

    if (_target is null)
      return;

    if (_overflow)
      ForceNeutralPerspective(delta);
    else
      LockOnMouseMovement(delta);
  }

  private void HandleInput(double delta)
  {
    if (Input.IsActionPressed("LockOn"))
    {
      if (_target is not null)
        return;

      _target = FindTarget();

      if (_target is null)
        LerpToDefaults(delta);
    }
    else
    {
      _target?.LockOnMarker?.HideMarker();
      _target = null;

      LerpToDefaults(delta);

      QuitOverflow();
    }
  }

  private ILockOnMarkerBearer? FindTarget()
  {
    if (_eyesight is null)
      return null;

    ILockOnMarkerBearer? target = null;

    float bestDistance = float.PositiveInfinity;

    foreach (Node node in _eyesight!.GetOverlappingBodies())
    {
      if (node is not ILockOnMarkerBearer markerBearer)
        continue;

      float candidateDistance = (markerBearer.GlobalPosition - _cameraPivot!.GlobalPosition).Length();

      if (candidateDistance < bestDistance)
      {
        target = markerBearer;

        bestDistance = candidateDistance;
      }
    }

    target?.LockOnMarker?.ShowMarker();

    return target;
  }

  private void ForceNeutralPerspective(double delta)
  {
    _cameraPivot!.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);

    Quaternion currentOrientation = _cameraPivot.GlobalBasis.GetRotationQuaternion();

    Transform3D newTransform = _cameraPivot.GlobalTransform.LookingAt(_target!.GlobalPosition);
    Quaternion newOrientation = newTransform.Basis.GetRotationQuaternion();

    if (
      currentOrientation.IsRoughly(newOrientation, tolerance: .01f)
      || !Input.GetLastMouseVelocity().IsRoughlyZero(tolerance: .01f)
    )
      QuitOverflow();

    _cameraPivot.GlobalBasis = new(currentOrientation.Slerp(to: newOrientation, weight: 5f * (float)delta));
  }

  private void LockOnMouseMovement(double delta)
  {
    _camera!.Fov = _camera.Fov.LerpF(to: _initialFov + 2f, weight: 5f * (float)delta);

    Vector3 forward3D = -_cameraPivot!.Basis.Z;
    Vector2 forward2D = new(forward3D.X, forward3D.Z);

    Vector3 difVector3D = _target!.GlobalPosition - _cameraPivot.GlobalPosition;
    Vector2 difVector2D = new(difVector3D.X, difVector3D.Z);

    float angle = forward2D.AngleTo(difVector2D); // Radians.

    if (angle.BetweenRadians(-1f / 4f, 1f / 4f))
    {
      _cameraSpring!.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + .5f, weight: 5f * (float)delta);

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);

      _timeFacingTheWrongWay = GameTime.Zero;
    }
    else if (angle.BetweenRadians(-7f / 12f, 7f / 12f))
    {
      float amplitude = difVector2D.Length();

      _cameraSpring!.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + .1f * amplitude, weight: 5f * (float)delta);

      Vector3 newPivotPosition = _initialPivotPosition + _cameraPivot.ToLocal(difVector3D with { Y = 0f }).Normalized() * .3f * amplitude;
      newPivotPosition = newPivotPosition with { Y = _cameraPivot.Position.Y };
      newPivotPosition = newPivotPosition.Rotated(Vector3.Up, _cameraPivot.Rotation.Y);

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: newPivotPosition, weight: 5f * (float)delta);

      _timeFacingTheWrongWay = GameTime.Zero;
    }
    else
    {
      if (Input.GetLastMouseVelocity().IsRoughlyZero(tolerance: .01f))
        _timeFacingTheWrongWay.Frames++;

      LerpToDefaults(delta);
    }
  }

  private void LerpToDefaults(double delta)
  {
    _cameraPivot!.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);
    _cameraSpring!.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength, weight: 5f * (float)delta);
    _camera!.Fov = _camera.Fov.LerpF(to: _initialFov, weight: 5f * (float)delta);
  }

  private void QuitOverflow()
  {
    _timeFacingTheWrongWay = GameTime.Zero;
    _overflow = false;
  }
}
