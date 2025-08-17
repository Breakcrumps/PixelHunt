using Godot;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class StopState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private GravityComposite? _gravityComposite;

  internal override void Enter()
  {
    if (_enemyChar is not null)
      _enemyChar.Velocity = Vector3.Zero;

    if (_gravityComposite is not null)
      _gravityComposite.CanGravitate = false;
  }

  internal override void Exit()
  {
    if (_gravityComposite is null)
      return;

    _gravityComposite.CanGravitate = true;
  }
}
