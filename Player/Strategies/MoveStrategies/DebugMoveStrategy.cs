using Godot;

[GlobalClass]
public partial class DebugMoveStrategy : State
{
  [Export] private Player? _character;
  [Export] private Node3D? _cameraPivot;
  [Export] private CollisionShape3D? _collision;

  [ExportGroup("Parameters")]
  [Export] private float _debugWalkSpeed = 5000f;
  [Export] private float _debugRunSpeed = 10_000f;
  [Export] private float _hoverVelocity = 1000f;

  public override void Enter()
  {
    if (_collision is not null)
      _collision.Disabled = true;
  }

  public override void PhysicsProcess(double delta)
  {
    Vector2 groundVelocity = GroundVelocity();
    float verticalVelocity = VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);
  }

  public override void Exit()
  {
    if (_collision is not null)
      _collision.Disabled = false;
  }

  private float VerticalVelocity() => Input.GetAxis("Dip", "Jump") * _hoverVelocity;

  private Vector2 GroundVelocity()
  {
    float speed = Input.IsActionPressed("Run") ? _debugRunSpeed : _debugWalkSpeed;

    float xDirection = Input.GetAxis("Left", "Right");
    float yDirection = -Input.GetAxis("Down", "Up");

    Vector2 direction = new(xDirection, yDirection);

    return direction.Normalized() * speed;
  }

  private void ApplyVelocity(Vector2 groundVelocity, float verticalVelocity)
  {
    _character!.Velocity = new(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _character.Velocity = _character.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
  }
}