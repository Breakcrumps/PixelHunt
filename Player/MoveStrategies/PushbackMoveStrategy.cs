using Godot;
using GameSrc.Parents;

namespace GameSrc.Player.MoveStrategies;

[GlobalClass]
internal sealed partial class PushbackMoveStrategy : State
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  internal Vector3 PushbackDirection { private get; set; }

  internal override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    _playerChar!.Velocity = PushbackDirection * _animHelper!.Speed;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (Flags.Debug)
      GD.Print("PhysicsProcess called on PushbackMoveStrategy");

    _playerChar!.Velocity = PushbackDirection * _animHelper!.Speed;
  }
}
