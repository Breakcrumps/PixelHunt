using PixelHunt.Animation;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Player;
using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class PushbackState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  internal Vector3 PushbackDirection { private get; set; }

  private PlayerChar? _playerChar;

  internal override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    if (_enemyChar is null || _animHelper is null)
      return;

    _enemyChar.Velocity = PushbackDirection * _animHelper.Speed;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_enemyChar is null)
      return;

    if (_animHelper is null)
      _enemyChar.Velocity *= .9f;
    else
      _enemyChar.Velocity = PushbackDirection * _animHelper.Speed;

    _enemyChar.AlignBody(delta, inverse: true);
  }
}
