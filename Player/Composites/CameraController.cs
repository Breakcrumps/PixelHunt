using Godot;

[GlobalClass]
internal partial class CameraController : Node
{
  [Export] private SpringArm3D? _cameraSpring;
  [Export] private Node3D? _cameraPivot;
  [Export] private CharacterBody3D? _character;

  [ExportGroup("Parameters")]
  [Export(PropertyHint.Range, "0f, .01f")] private float _mouseSensitivity = .01f;
  [Export(PropertyHint.Range, "10f, 90f")] private float _tiltLimit = 75f;
  [Export] private float _turnSpeed = 5f;
  [Export] private float _zoomSpeed = 1f;

  public override void _Ready()
  {
    _tiltLimit = Mathf.DegToRad(_tiltLimit);
  }

  public override void _PhysicsProcess(double delta)
  {
    HandleZoom(delta);

    MoveByArrows();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is not InputEventMouseMotion mouseMotion)
      return;

    Vector2 motion = mouseMotion.Relative;
    Vector3 newRotation = _cameraPivot!.Rotation;

    newRotation.X -= motion.Y * _mouseSensitivity;
    newRotation.X = Mathf.Clamp(newRotation.X, -_tiltLimit, _tiltLimit);
    newRotation.Y -= motion.X * _mouseSensitivity;

    _cameraPivot.Rotation = newRotation;
  }

  private void HandleZoom(double delta)
  {
    if (Input.IsActionJustReleased("WheelUp"))
    {
      _cameraSpring!.SpringLength = Mathf.Clamp(
          Mathf.Lerp(
            _cameraSpring.SpringLength,
            _cameraSpring.SpringLength - 1f,
            _zoomSpeed * (float)delta
          ),
          0f,
          10f
      );
    }
    else if (Input.IsActionJustReleased("WheelDown"))
    {
      _cameraSpring!.SpringLength = Mathf.Clamp(
          Mathf.Lerp(
            _cameraSpring.SpringLength,
            _cameraSpring.SpringLength + 1f,
            _zoomSpeed * (float)delta
          ),
          0f,
          10f
      );
    }
  }

  private void MoveByArrows()
  {
    float xDirection = Input.GetAxis("LeftArrow", "RightArrow");
    float yDirection = Input.GetAxis("DownArrow", "UpArrow");

    Vector3 newRotation = _cameraPivot!.Rotation;

    newRotation.X += yDirection * .1f;
    newRotation.X = Mathf.Clamp(newRotation.X, -_tiltLimit, _tiltLimit);
    newRotation.Y -= xDirection * .1f;

    _cameraPivot.Rotation = newRotation;
  }
}