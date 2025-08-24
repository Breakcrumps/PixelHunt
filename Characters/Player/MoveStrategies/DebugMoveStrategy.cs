using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class DebugMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private CameraPivot? _cameraPivot;
  [Export] private CollisionShape3D? _collision;
  [Export] private MoveStateMachine? _moveStateMachine;


  [ExportGroup("Parameters")]
  [Export] private float _debugWalkSpeed = 5000f;
  [Export] private float _debugRunSpeed = 10_000f;
  [Export] private float _hoverVelocity = 1000f;

  internal override void Enter()
  {
    if (_collision is not null)
      _collision.Disabled = true;
  }

  internal override void PhysicsProcess(double delta)
  {
    Vector2 groundVelocity = GroundVelocity();
    float verticalVelocity = VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);
  }

  internal override void UnhandledInput(InputEvent @event)
  {
    if (!@event.IsActionPressed("Debug"))
      return;

    _moveStateMachine?.Transition("FreeMoveStrategy");
  }

  internal override void Exit()
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
    _playerChar!.Velocity = new Vector3(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _playerChar.Velocity = _playerChar.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
  }
}
