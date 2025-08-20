using Godot;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class BodyAligner : Node
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Skeleton3D? _armature;

  [ExportGroup("Parameters")]
  [Export] private float _turnSpeed = 10f;

  public override void _PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    AlignBody(delta);
  }

  private void AlignBody(double delta)
  {
    if (_armature is null)
      return;

    Vector2 horizontalVelocity = new(_playerChar!.Velocity.X, _playerChar.Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _armature.Rotation = _armature.Rotation with
    {
      Y = Mathf.LerpAngle(
        from: _armature.Rotation.Y,
        to: horizontalVelocity.AngleTo(Vector2.Down),
        weight: _turnSpeed * (float)delta
      )
    };
  }
}
