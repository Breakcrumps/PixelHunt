using PixelHunt.Animation;
using PixelHunt.Parents;
using Godot;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Enemy.Composites;

namespace PixelHunt.Characters.Enemy.States;

[GlobalClass]
internal sealed partial class AttackState : State
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyAligner? _enemyAligner;
  [Export] private Animator? _animator;

  private PlayerChar? _playerChar;

  public override void _Ready()
  {
    _playerChar = (PlayerChar)GetTree().GetFirstNodeInGroup("Player");
  }

  internal override void Enter()
  {
    if (_enemyChar is null)
      return;

    _animator?.PlayAnimation("Attack");
    _enemyChar!.Velocity = Vector3.Zero;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    Vector2 horizontalPosition = new(_playerChar.GlobalPosition.X, _playerChar.GlobalPosition.Z);
    
    _enemyAligner?.AlignBody(horizontalPosition, delta);
  }
}
