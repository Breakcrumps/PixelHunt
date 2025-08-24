using Godot;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class CameraPivot : Node3D
{
  [Export(PropertyHint.Range, "0f, .01f")] private float _mouseSensitivity = .01f;
  [Export(PropertyHint.Range, "10f, 90f")] private float _tiltLimit = 75f; // Degrees.
  [Export] private float _turnSpeed = 5f;
  [Export] private float _zoomSpeed = 10f;

  private bool _canControlCamera = true;

  public override void _UnhandledInput(InputEvent @event)
    => HandleMouseMovement(@event);

  public override void _Ready()
    => _tiltLimit *= Mathf.Pi / 180f; // Radians.

  private void HandleMouseMovement(InputEvent @event)
  {
    if (!_canControlCamera)
      return;
    
    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    Vector2 motion = mouseMotion.Relative;

    Vector3 newRotation = Rotation;

    newRotation.X -= motion.Y * _mouseSensitivity;
    newRotation.X = Mathf.Clamp(newRotation.X, min: -_tiltLimit, max: _tiltLimit);
    newRotation.Y -= motion.X * _mouseSensitivity;

    Rotation = newRotation;
  }

  public override void _PhysicsProcess(double delta)
  {
    _canControlCamera = !Input.IsActionPressed("Aim");
    
    Rotation = Rotation with { Z = 0f };
  }
}
