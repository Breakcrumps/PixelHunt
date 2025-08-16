using Godot;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class StopMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;

  internal override void Enter()
  {
    if (_playerChar is null)
      return;

    _playerChar.Velocity = _playerChar.Velocity with { X = 0f, Z = 0f };
  }

  internal override void PhysicsProcess(double delta)
    => ApplyGravity();

  private void ApplyGravity()
  {
    if (_playerChar is null)
      return;

    if (_playerChar.IsOnFloor())
      return;

    _playerChar.Velocity = _playerChar.Velocity with { Y = _playerChar.Velocity.Y - 3f };
  }
}
