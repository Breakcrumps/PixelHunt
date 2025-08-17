using Godot;

namespace PixelHunt.Characters.Enemy.Composites;

[GlobalClass]
internal sealed partial class GravityComposite : Node
{
  [Export] private EnemyChar? _enemyChar;

  [ExportGroup("Parameters")]
  private float _g = 1f;
  
  internal bool CanGravitate { private get; set; } = true;

  public override void _PhysicsProcess(double delta)
  {
    if (!CanGravitate)
      return;

    if (_enemyChar is null)
      return;

    if (_enemyChar.IsOnFloor())
      return;

    _enemyChar.Velocity = _enemyChar.Velocity with { Y = _enemyChar.Velocity.Y - _g };
  }
}
