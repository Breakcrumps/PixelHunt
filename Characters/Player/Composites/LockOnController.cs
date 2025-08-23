using Godot;
using PixelHunt.Characters.Enemy;
using PixelHunt.Mechanics.Markers;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.Composites;

// I hate this one more than my life.
[GlobalClass]
internal sealed partial class LockOnController : Node
{
  [Export] private Area3D? _eyesight;
  [Export] private Node3D? _cameraPivot;
  [Export] private SpringArm3D? _cameraSpring;
  [Export] private PlayerChar? _playerChar;
  [Export] private Camera3D? _camera;

  private Vector3 _initialPivotPosition;
  private float _initialSpringLength;
  private float _initialFov;

  private GameTime _timeFacingTheWrongWay = GameTime.Zero; // Genius name.
  private bool _overflow;

  private EnemyChar? _targetChar;

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

    if (_targetChar is null)
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
      if (_targetChar is not null)
        return;

      _targetChar = FindTarget();

      if (_targetChar is null)
        LerpToDefaults(delta);
    }
    else
    {
      _targetChar = null;

      LerpToDefaults(delta);

      QuitOverflow();
    }
  }

  private EnemyChar? FindTarget()
  {
    if (_eyesight is null)
      return null;

    EnemyChar? target = null;

    foreach (Node node in _eyesight!.GetOverlappingBodies())
    {
      if (node is not EnemyChar enemyChar)
        continue;

      if (_targetChar is null)
      {
        target = enemyChar;
        continue;
      }

      Vector3 currentDistance = _targetChar.GlobalPosition - _cameraPivot!.GlobalPosition;
      Vector3 candidateDistance = enemyChar.GlobalPosition - _cameraPivot.GlobalPosition;

      if (candidateDistance.Length() < currentDistance.Length())
      {
        target = enemyChar;
      }
    }

    target?.LockOnMarker?.ShowMarker();

    return target;
  }

  private void ForceNeutralPerspective(double delta)
  {
    _cameraPivot!.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);

    Quaternion currentOrientation = _cameraPivot.GlobalBasis.GetRotationQuaternion();

    Transform3D newTransform = _cameraPivot.GlobalTransform.LookingAt(_targetChar!.GlobalPosition);
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

    Vector3 difVector3D = _targetChar!.GlobalPosition - _cameraPivot.GlobalPosition;
    Vector2 difVector2D = new(difVector3D.X, difVector3D.Z);

    float angle = forward2D.AngleTo(difVector2D); // Radians.

    if (angle.BetweenRadians(-1f / 4f, 1f / 4f))
    {
      _cameraSpring!.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + .5f, weight: 5f * (float)delta);

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);
    }
    else
    {
      float amplitude = difVector2D.Length();

      _cameraSpring!.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + .1f * amplitude, weight: 5f * (float)delta);

      Vector3 newPivotPosition = _initialPivotPosition + _cameraPivot.ToLocal(difVector3D with { Y = 0f }).Normalized() * .3f * amplitude;
      newPivotPosition = newPivotPosition with { Y = _cameraPivot.Position.Y };
      newPivotPosition = newPivotPosition.Rotated(Vector3.Up, _cameraPivot.Rotation.Y);

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: newPivotPosition, weight: 5f * (float)delta);
    }

    if (!angle.BetweenRadians(-7f / 12f, 7f / 12f) && Input.GetLastMouseVelocity().IsRoughlyZero(tolerance: .01f))
      _timeFacingTheWrongWay.Frames++;
    else
      _timeFacingTheWrongWay = GameTime.Zero;
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
