using PixelHunt.Animation;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;
using PixelHunt.Static;
using Godot;
using System.Diagnostics;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class FreeMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Node3D? _cameraPivot;
  [Export] private Skeleton3D? _armature;
  [Export] private PlayerAnimator? _playerAnimator;
  [Export] private AnimationHelper? _animHelper;
  [Export] private MoveStateMachine? _moveStateMachine;


  [ExportGroup("Parameters")]
  [Export] private float _jumpHeight = 400f;
  [Export] private float _jumpTime = .45f; // Seconds.
  [Export] private float _doubleJumpHeight = 300f;
  private float _jumpVelocity;
  private float _doubleJumpVelocity;
  private float _g;

  private bool _slowWalk;
  private int _doubleJumps;

  private readonly Stopwatch _tempWatch = new();

  public override void _Ready()
  {
    _jumpTime *= Engine.PhysicsTicksPerSecond; // Frames.

    _jumpVelocity = 2 * _jumpHeight / (.5f * _jumpTime);
    _g = _jumpVelocity / (.5f * _jumpTime);

    _doubleJumpVelocity = Mathf.Sqrt(2 * _doubleJumpHeight * _g);

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"{_jumpVelocity} {_g} {_doubleJumpVelocity}");
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar!.IsOnFloor())
      _doubleJumps = 1;

    Vector2 groundVelocity = GroundVelocity();
    float verticalVelocity = VerticalVelocity();

    ApplyVelocity(groundVelocity, verticalVelocity);

    AnimateMovement();
  }

  internal override void UnhandledInput(InputEvent @event)
  {
    HandleSlowWalk(@event);

    HandleDebug(@event);
  }

  private float VerticalVelocity()
  {
    float yVelocity = _playerChar!.IsOnFloor() ? 0f : _playerChar.Velocity.Y - _g;

    if (DebugFlags.GetDebugFlag(this) && _playerChar.IsOnFloor())
    {
      if (_tempWatch.IsRunning)
      {
        GD.Print($"Time in air: {_tempWatch.Elapsed.Milliseconds}");
        _tempWatch.Stop();
      }
    }

    if (Input.IsActionJustPressed("Jump"))
    {
      if (_playerChar.IsOnFloor())
      {
        yVelocity = _jumpVelocity;
        _tempWatch.Restart();
      }
      else if (_doubleJumps != 0)
      {
        yVelocity = _doubleJumpVelocity;
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
    _playerChar!.Velocity = new Vector3(groundVelocity.X, verticalVelocity, groundVelocity.Y);

    _playerChar.Velocity = _playerChar.Velocity.Rotated(Vector3.Up, _cameraPivot!.Rotation.Y);
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

    _playerAnimator!.CanProcessRequests = true;
    
    _playerAnimator.PlayAnimation(animation);
  }
}
