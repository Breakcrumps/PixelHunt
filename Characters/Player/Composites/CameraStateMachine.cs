using Godot;
using PixelHunt.Parents;
using PixelHunt.Static;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class CameraStateMachine : StateMachine
{
  [Export] private SpringArm3D? _cameraSpring;

  [ExportGroup("Parameters")]
  [Export(PropertyHint.Range, "10f, 90f")] internal float TiltLimit { get; private set; } = 75f;
  [Export] private float _turnSpeed = 5f;
  [Export] private float _zoomSpeed = 10f;

  public override void _Ready()
  {
    TiltLimit = Mathf.DegToRad(TiltLimit);

    FillStates();

    Transition("FreeCameraMode");

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"Initial camera state: {CurrentState?.Name}");
  }

  public override void _PhysicsProcess(double delta)
  {
    HandleZoom(delta);
  }

  private void HandleZoom(double delta)
  {
    if (Input.IsActionJustReleased("WheelUp"))
    {
      _cameraSpring!.SpringLength = Mathf.Clamp(
        Mathf.Lerp(
          from: _cameraSpring.SpringLength,
          to: _cameraSpring.SpringLength - 1f,
          weight: _zoomSpeed * (float)delta
        ),
        min: 0f,
        max: 10f
      );
    }
    else if (Input.IsActionJustReleased("WheelDown"))
    {
      _cameraSpring!.SpringLength = Mathf.Clamp(
        Mathf.Lerp(
          from: _cameraSpring.SpringLength,
          to: _cameraSpring.SpringLength + 1f,
          weight: _zoomSpeed * (float)delta
        ),
        min: 0f,
        max: 10f
      );
    }
  }
}
