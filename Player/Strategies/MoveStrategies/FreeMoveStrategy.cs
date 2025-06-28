using Godot;

[GlobalClass]
public partial class FreeMoveStrategy : State
{
  [Export] private Player? _character;
  [Export] private Node3D? _cameraPivot;
  [Export] private Node3D? _armature;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;
  [Export] private MoveStateMachine? _moveStateMachine;


  [ExportGroup("Parameters")]
  [Export] private float _turnSpeed = 10f;
  [Export] private float _jumpVelocity = 100f;
  [Export] private float _g = 9.8f;

  private bool _slowWalk;
  private int _doubleJumps;

  public override void PhysicsProcess(double delta)
  {
    if (_character!.IsOnFloor())
      _doubleJumps = 1;

    Vector2 groundVelocity = GroundVelocity();
    float verticalVelocity = VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);

    AlignBody(delta);

    AnimateMovement();
  }

  public override void UnhandledInput(InputEvent @event)
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

  private Vector2 GroundVelocity()
  {
    if (_animHelper is null)
      return Vector2.Zero;

    float xDirection = Input.GetAxis("Left", "Right");
    float yDirection = -Input.GetAxis("Down", "Up");

    Vector2 direction = new(xDirection, yDirection);

    return direction.Normalized() * _animHelper.Speed;
  }


  private void HandleSlowWalk(InputEvent @event)
  {
    if (!@event.IsActionReleased("SlowWalk"))
      return;

    _slowWalk = !_slowWalk;
  }

  private void HandleDebug(InputEvent @event)
  {
    if (!@event.IsActionPressed("Debug"))
      return;

    _moveStateMachine?.Transition("DebugMoveStrategy");
  }

  private void ApplyVelocity(Vector2 groundVelocity, float verticalVelocity)
  {
    _character!.Velocity = new(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _character.Velocity = _character.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
  }

  private void AlignBody(double delta)
  {
    Vector2 horizontalVelocity = new(_character!.Velocity.X, _character.Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _armature!.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        _armature.Rotation.Y,
        horizontalVelocity.AngleTo(new Vector2(0, 1)),
        _turnSpeed * (float)delta
      )
    };
  }
  
  public void AnimateMovement()
  {
    if (_character is null)
      return;

    Vector2 horizontalVelocity = new(_character.Velocity.X, _character.Velocity.Z);

    string animation = (
      !_character.IsOnFloor()
      ? _character.Velocity.Y == 0 ? "Hover"
      : _character.Velocity.Y > 0 ? "Rise" : "Fall"
      : horizontalVelocity != Vector2.Zero
      ? _slowWalk ? "Walk" : "Run"
      : "Idle"
    );

    _animator?.PlayAnimation(animation);
  }
}