using Godot;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class CameraController : Node
{
  [Export] private SpringArm3D? _cameraSpring;
  [Export] private Node3D? _cameraPivot;
  [Export] private CharacterBody3D? _character;

  [ExportGroup("Parameters")]
  [Export(PropertyHint.Range, "0f, .01f")] private float _mouseSensitivity = .01f;
  [Export(PropertyHint.Range, "10f, 90f")] private float _tiltLimit = 75f;
  [Export] private float _turnSpeed = 5f;
  [Export] private float _zoomSpeed = 10f;

  public override void _UnhandledInput(InputEvent @event)
    => HandleMouseMovement(@event);

  private void HandleMouseMovement(InputEvent @event)
  {
    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    Vector2 motion = mouseMotion.Relative;
    Vector3 newRotation = _cameraPivot!.Rotation;

    newRotation.X -= motion.Y * _mouseSensitivity;
    newRotation.X = Mathf.Clamp(newRotation.X, min: -_tiltLimit, max: _tiltLimit);
    newRotation.Y -= motion.X * _mouseSensitivity;

    _cameraPivot.Rotation = newRotation;
  }
}
