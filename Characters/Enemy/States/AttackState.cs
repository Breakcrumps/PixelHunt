using PixelHunt.Animation;
using PixelHunt.Parents;
using Godot;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class AttackState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private Animator? _animator;

  internal override void Enter()
  {
    _animator?.PlayAnimation("Attack");
    _enemyChar!.Velocity = Vector3.Zero;
  }
}
