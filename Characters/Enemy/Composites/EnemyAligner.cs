using Godot;

namespace PixelHunt.Characters.Enemy.Composites;

[GlobalClass]
internal sealed partial class EnemyAligner : Node
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private Node3D? _armature;

  [ExportGroup("Parameters")]
  [Export] private float _alignSpeed = 10f;

  [Export] private bool _canAlign = true;

  internal void AlignBody(Vector2 target, double delta, bool inverse = false)
  {
    if (!_canAlign)
      return;

    if (_enemyChar is null)
      return;

    Vector2 refVector = (
      inverse
      ? Vector2.Up
      : Vector2.Down
    );

    Vector2 difVector = target - new Vector2(_enemyChar.GlobalPosition.X, _enemyChar.GlobalPosition.Z);

    _armature!.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: difVector.AngleTo(refVector),
        weight: _alignSpeed * (float)delta
      )
    };
  }

  internal void AlignBodyToVelocity(double delta, bool inverse = false)
  {
    if (!_canAlign)
      return;

    if (_enemyChar is null)
      return;
    
    Vector2 horizontalVelocity = new(_enemyChar.Velocity.X, _enemyChar.Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    Vector2 refVector = (
      inverse
      ? Vector2.Up
      : Vector2.Down
    );

    _armature!.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: horizontalVelocity.AngleTo(refVector),
        weight: _alignSpeed * (float)delta
      )
    };
  }
}
