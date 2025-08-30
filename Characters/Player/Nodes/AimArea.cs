using Godot;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Mechanics.Markers;

using static Godot.Mathf;

namespace PixelHunt.Mechanics.Aim;

[GlobalClass]
internal sealed partial class AimArea : Area3D
{
  [Export] private MoveStateMachine? _moveStateMachine;
  [Export] private RayCast3D? _rayCast;
  
  [Export] private float _threshold = 2f; 
  
  internal IAimMarkerBearer? Target { get; set; }

  internal bool CanRetarget { private get; set; } = true;

  public override void _PhysicsProcess(double delta)
  {
    if (!Input.IsActionPressed("Aim"))
      return;

    RelinquishTarget();
    Target = DetermineTarget();
  }

  private IAimMarkerBearer? DetermineTarget()
  {
    if (_rayCast is null)
      return null;
    
    IAimMarkerBearer? target = null;

    float bestError = Target is null ? float.PositiveInfinity : GetError(Target);
    float bestDistance = Target is null ? float.PositiveInfinity : GetDistance(Target);

    foreach (Node node in GetOverlappingBodies())
    {
      if (node is not IAimMarkerBearer candidate)
        continue;

      _rayCast.TargetPosition = _rayCast.ToLocal(candidate.GlobalPosition);
      _rayCast.ForceRaycastUpdate();

      if (_rayCast.GetCollider() != candidate)
        continue;

      if (
        GetError(candidate) < bestError
        || GetError(candidate) == bestError && GetDistance(candidate) < bestDistance
      )
      {
        target = candidate;

        bestError = GetError(candidate);
        bestDistance = GetDistance(candidate);
      }
    }

    target?.AimMarker?.ShowMarker();
    return target;
  }

  private void RelinquishTarget()
  {
    Target?.AimMarker?.HideMarker();
    Target = null;
  }

  private float GetError(IAimMarkerBearer candidate)
  {
    Vector3 hypotenuse = candidate.GlobalPosition - GlobalPosition;
    Vector3 cathetus = -GlobalBasis.Z;

    return hypotenuse.Length() * Sin(hypotenuse.AngleTo(cathetus));
  }

  // private float GetError(IAimMarkerBearer candidate)
  // {
  //   Vector3 difVector = candidate.GlobalPosition - GlobalPosition;
  //   Vector3 forward = -GlobalBasis.Z;

  //   Vector2 hypotenuse = new(difVector.X, difVector.Z);
  //   Vector2 cathetus = new(forward.X, forward.Z);

  //   return hypotenuse.Length() * Sin(hypotenuse.AngleTo(cathetus));
  // }

  private float GetDistance(IAimMarkerBearer candidate)
    => (candidate.GlobalPosition - GlobalPosition).Length();

  public override void _UnhandledInput(InputEvent @event)
  {
    HandleAreaRotation(@event);

    if (@event.IsActionPressed("Homing") && Target is not null)
      _moveStateMachine?.Transition("HomingMoveStrategy");
  }

  private void HandleAreaRotation(InputEvent @event)
  {
    if (!CanRetarget)
      return;
    
    if (!Input.IsActionPressed("Aim"))
      return;

    if (@event is not InputEventMouseMotion mouseEvent)
      return;

    Vector2 mouseMotion = mouseEvent.Relative;

    if (mouseMotion.Length() < _threshold)
      return;

    Rotation = new Vector3(0f, -(mouseMotion.Angle() + Pi / 2f), 0f);
  }
}
