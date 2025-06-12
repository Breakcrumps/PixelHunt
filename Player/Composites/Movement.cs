using Godot;

[GlobalClass]
public partial class Movement : Node
{
  [Export] private CharacterBody3D? _character;
  [Export] private Node3D? _cameraPivot;

  [Export] private float _walkSpeed = 50f;
  [Export] private float _runSpeed = 100f;
  [Export] private float _slowWalkSpeed = 10f;
  [Export] private float _debugWalkSpeed = 5000f;
  [Export] private float _debugRunSpeed = 10_000f;
  [Export] private float _jumpVelocity = 200f;
  [Export] private float _hoverVelocity = 1000f;
  [Export] private float _g = 15f;

  private bool _slowWalk;
  private bool _isInDebugMode;
  private int _doubleJumps;

  public void Move()
  {
    if (_character!.IsOnFloor())
        _doubleJumps = 1;

    Vector2 groundVelocity = _isInDebugMode ? DebugGroundVelocity() : GroundVelocity();
    float verticalVelocity = _isInDebugMode ? DebugVerticalVelocity() : VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);

    _character.MoveAndSlide();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    HandleSlowWalk(@event);

    HandleDebug(@event);
  }

  private float VerticalVelocity()
  {
    float yVelocity = _character!.IsOnFloor() ? 0f : _character.Velocity.Y - _g;

    if (Input.IsActionJustPressed("Jump"))
    {
      if (_character.IsOnFloor())
      {
        yVelocity = _jumpVelocity;
      }
      else if (_doubleJumps != 0)
      {
        yVelocity = _jumpVelocity;
        _doubleJumps--;
      }
    }

    return yVelocity;
  }

  private float DebugVerticalVelocity() => Input.GetAxis("Dip", "Jump") * _hoverVelocity;

  private Vector2 GroundVelocity()
  {
    float speed = (
      _slowWalk
      ? _slowWalkSpeed
      : Input.IsActionPressed("Run")
      ? _runSpeed
      : _walkSpeed
    );

    float xDirection = Input.GetAxis("Left", "Right");
    float yDirection = -Input.GetAxis("Down", "Up");

    Vector2 direction = new(xDirection, yDirection);

    return direction.Normalized() * speed;
  }

  private Vector2 DebugGroundVelocity()
  {
    float speed = Input.IsActionPressed("Run") ? _debugRunSpeed : _debugWalkSpeed;

    float xDirection = Input.GetAxis("Left", "Right");
    float yDirection = -Input.GetAxis("Down", "Up");

    Vector2 direction = new(xDirection, yDirection);

    return direction.Normalized() * speed;
  }

  private void HandleSlowWalk(InputEvent @event)
  {
    if (!@event.IsActionPressed("SlowWalk"))
      return;

    _slowWalk = !_slowWalk;
  }

  private void HandleDebug(InputEvent @event)
  {
    if (!@event.IsActionPressed("Debug"))
      return;
    
    _isInDebugMode = !_isInDebugMode;
    GetNode<CollisionShape3D>("%Hurtbox").Disabled = _isInDebugMode;
    GetNode<CollisionShape3D>("%Legs").Disabled = _isInDebugMode;
  }

  private void ApplyVelocity(Vector2 groundVelocity, float verticalVelocity)
  {
    _character!.Velocity = new(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _character.Velocity = _character.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
  }
}