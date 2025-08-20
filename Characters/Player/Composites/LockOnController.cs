using Godot;
using PixelHunt.Characters.Enemy;
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

  private Vector3 _initialPivotPosition;
  private float _initialSpringLength;

  private GameTime _timeFacingTheWrongWay = GameTime.Zero; // Genius name.
  private bool _overflow;

  private Character? _targetChar;

  public override void _Ready()
  {
    if (_cameraPivot is not null)
      _initialPivotPosition = _cameraPivot.Position;

    if (_cameraSpring is not null)
      _initialSpringLength = _cameraSpring.SpringLength;
  }

  public override void _PhysicsProcess(double delta)
  {
    HandleInput();

    if (_overflow)
      ForceNeutralPerspective(delta);
    else
      LockOnMouseMovement(delta);
  }

  private void ForceNeutralPerspective(double delta)
  {
    if (_cameraPivot is null)
      return;

    if (_targetChar is null)
      return;

    _cameraPivot.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);

    Vector3 difVector3D = _targetChar.GlobalPosition - _cameraPivot.GlobalPosition;
    Vector2 difVector2D = new(difVector3D.X, difVector3D.Z);

    float resultAngle = difVector2D.Angle() + Mathf.Pi / 2f;

    _cameraPivot.Rotation = _cameraPivot.Rotation with
    {
      Y = _cameraPivot.Rotation.Y.LerpF(to: resultAngle, weight: 5f * (float)delta)
    };

    if (_cameraPivot.Rotation.Y.IsRoughly(resultAngle, tolerance: .1f))
    {
      _timeFacingTheWrongWay = GameTime.Zero;
      _overflow = false;
    }
  }

  private void LockOnMouseMovement(double delta)
  {
    if (_cameraPivot is null)
      return;

    if (_cameraSpring is null)
      return;

    if (_targetChar is null)
    {
      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 10f * (float)delta);
      _cameraSpring.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength, weight: 10f * (float)delta);

      return;
    }

    Vector3 forward3D = -_cameraPivot.Basis.Z;
    Vector2 forward2D = new(forward3D.X, forward3D.Z);

    Vector3 difVector3D = _targetChar.GlobalPosition - _cameraPivot.GlobalPosition;
    Vector2 difVector2D = new(difVector3D.X, difVector3D.Z);

    float angle = forward2D.AngleTo(difVector2D); // Radians.

    if (-Mathf.Pi / 4f <= angle && angle <= Mathf.Pi / 4f)
    {
      _cameraSpring.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + 1f, weight: 5f * (float)delta);

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: _initialPivotPosition, weight: 5f * (float)delta);
    }
    else
    {
      _cameraSpring.SpringLength = _cameraSpring.SpringLength.LerpF(to: _initialSpringLength + 2f, weight: 5f * (float)delta);

      Vector3 newPivotPosition = _initialPivotPosition + (difVector3D with { Y = 0f }).Normalized();
      newPivotPosition = _cameraPivot.ToLocal(newPivotPosition) with { Y = _cameraPivot.Position.Y };

      _cameraPivot.Position = _cameraPivot.Position.Lerp(to: newPivotPosition, weight: (float)delta);
    }

    if (angle <= -2f / 3f * Mathf.Pi || angle >= 2f / 3f * Mathf.Pi)
    {
      _timeFacingTheWrongWay.Frames++;
    }
    else
    {
      _timeFacingTheWrongWay = GameTime.Zero;
    }

    _overflow = _timeFacingTheWrongWay == GameTime.Second;
  }


  private void HandleInput()
  {
    if (Input.IsActionPressed("LockOn"))
    {
      if (_targetChar is not null)
        return;

      _targetChar = FindTarget();
    }
    else
    {
      _targetChar = null;
      _timeFacingTheWrongWay = GameTime.Zero;
    }
  }

  private Character? FindTarget()
  {
    if (_cameraPivot is null)
      return null;

    if (_eyesight is null)
      return null;

    Character? target = null;

    foreach (Node node in _eyesight!.GetOverlappingBodies())
    {
      if (node is not EnemyChar enemyChar)
        continue;

      if (_targetChar is null)
      {
        target = enemyChar;
        continue;
      }

      Vector3 currentDistance = _targetChar.GlobalPosition - _cameraPivot.GlobalPosition;
      Vector3 candidateDistance = enemyChar.GlobalPosition - _cameraPivot.GlobalPosition;

      if (candidateDistance.Length() < currentDistance.Length())
      {
        target = enemyChar;
      }
    }

    return target;
  }
}
