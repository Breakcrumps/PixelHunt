using Godot;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.CameraModes;

[GlobalClass]
internal sealed partial class FreeCameraMode : State
{
  [Export] private CameraStateMachine? _cameraStateMachine;
  [Export] private Node3D? _cameraPivot;
  [Export] private CharacterBody3D? _character;

  [ExportGroup("Parameters")]
  [Export(PropertyHint.Range, "0f, .01f")] private float _mouseSensitivity = .01f;

  internal override void PhysicsProcess(double delta)
  {
    MoveByArrows();
  }

  internal override void UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("Run"))
    {
      _cameraStateMachine?.Transition("LockOnCameraMode");
      return;
    }
    
    HandleMouseMovement(@event);
  }

  private void HandleMouseMovement(InputEvent @event)
  {
    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    Vector2 motion = mouseMotion.Relative;
    Vector3 newRotation = _cameraPivot!.Rotation;

    newRotation.X -= motion.Y * _mouseSensitivity;
    newRotation.X = Mathf.Clamp(newRotation.X, min: -_cameraStateMachine!.TiltLimit, max: _cameraStateMachine.TiltLimit);
    newRotation.Y -= motion.X * _mouseSensitivity;

    _cameraPivot.Rotation = newRotation;
  }

  private void MoveByArrows()
  {
    float xDirection = Input.GetAxis("LeftArrow", "RightArrow");
    float yDirection = Input.GetAxis("DownArrow", "UpArrow");

    Vector3 newRotation = _cameraPivot!.Rotation;

    newRotation.X += yDirection * .1f;
    newRotation.X = Mathf.Clamp(newRotation.X, min: -_cameraStateMachine!.TiltLimit, max: _cameraStateMachine.TiltLimit);
    newRotation.Y -= xDirection * .1f;

    _cameraPivot.Rotation = newRotation;
  }
}
