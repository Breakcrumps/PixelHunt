using Godot;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Mechanics.Markers;

using static Godot.Mathf;

namespace PixelHunt.Mechanics.Aim;

[GlobalClass]
internal sealed partial class AimArea : Area3D
{
  [Export] private MoveStateMachine? _moveStateMachine;
  
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
    IAimMarkerBearer? target = null;

    float bestError = float.PositiveInfinity;
    float bestDistance = float.PositiveInfinity;

    foreach (Node node in GetOverlappingBodies())
    {
      if (node is not IAimMarkerBearer candidate)
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
