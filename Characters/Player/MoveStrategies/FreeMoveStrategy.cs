using GameSrc.Animation;
using GameSrc.Characters.Player.Composites;
using GameSrc.Parents;
using GameSrc.Static;
using Godot;

namespace GameSrc.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class FreeMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Node3D? _cameraPivot;
  [Export] private Node3D? _armature;
  [Export] private PlayerAnimator? _playerAnimator;
  [Export] private AnimationHelper? _animHelper;
  [Export] private MoveStateMachine? _moveStateMachine;


  [ExportGroup("Parameters")]
  [Export] private float _turnSpeed = 10f;
  [Export] private float _jumpVelocity = 100f;
  [Export] private float _g = 9.8f;

  private bool _slowWalk;
  private int _doubleJumps;

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar!.IsOnFloor())
      _doubleJumps = 1;

    Vector2 groundVelocity = GroundVelocity();
    float verticalVelocity = VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);

    AlignBody(delta);

    AnimateMovement();
  }

  internal override void UnhandledInput(InputEvent @event)
  {
    HandleSlowWalk(@event);

    HandleDebug(@event);

    HandleUnsheathe(@event);
  }

  private float VerticalVelocity()
  {
    float yVelocity = _playerChar!.IsOnFloor() ? 0f : _playerChar.Velocity.Y - _g;

    if (Input.IsActionJustPressed("Jump"))
    {
      if (_playerChar.IsOnFloor())
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

  private void HandleUnsheathe(InputEvent @event)
  {
    if (!@event.IsActionPressed("Interact"))
      return;

    _playerAnimator?.FlipUnsheathe();
  }

  private void ApplyVelocity(Vector2 groundVelocity, float verticalVelocity)
  {
    _playerChar!.Velocity = new Vector3(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _playerChar.Velocity = _playerChar.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
  }

  private void AlignBody(double delta)
  {
    Vector2 horizontalVelocity = new(_playerChar!.Velocity.X, _playerChar.Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _armature!.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: horizontalVelocity.AngleTo(new Vector2(0, 1)),
        weight: _turnSpeed * (float)delta
      )
    };
  }

  private void AnimateMovement()
  {
    if (_playerChar is null)
      return;

    if (_playerAnimator is null)
      return;

    if (_playerChar.IsOnFloor())
      AnimateOnGround();
    else
      AnimateInAir();
  }

  private void AnimateOnGround()
  {
    if (InputHelper.GetMovementDirection() != Vector2.Zero)
      _playerAnimator!.Run();
    else
      _playerAnimator!.StopOrIdle();
  }

  private void AnimateInAir()
  {
    string animation = (
      _playerChar!.Velocity.Y == 0 ? "Hover"
      : _playerChar.Velocity.Y > 0 ? "Rise" : "Fall"
    );

    _playerAnimator!.PlayAnimation(animation, bypass: true);

    _playerAnimator.CanProcessRequests = true;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print("Animating in air!");
  }
}
