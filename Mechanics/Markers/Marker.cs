using Godot;
using PixelHunt.Static;

namespace PixelHunt.Mechanics.Markers;

[GlobalClass]
internal sealed partial class Marker : Node3D
{
  [Export] private Texture2D? _texture;
  [Export] private Sprite3D? _sprite;
  [Export] private AnimationPlayer? _animPlayer;

  private bool _active;

  public override void _Ready()
  {
    if (_sprite is null)
      return;

    _sprite.Texture = _texture;
    _sprite.Visible = false;
  }

  internal void ShowMarker()
  {
    if (_animPlayer is null)
      return;

    if (_sprite is null)
      return;

    _active = true;
    _animPlayer.Play("Spin");
    _sprite.Visible = true;
  }

  internal void HideMarker()
  {
    if (_animPlayer is null)
      return;

    if (_sprite is null)
      return;

    _active = false;
    _animPlayer.Stop();
    _sprite.Visible = false;
  }

  public override void _Process(double delta)
  {
    if (!_active)
      return;

    if (GlobalInstances.PlayerCamera is not Camera3D camera)
      return;

    LookAt(camera.GlobalPosition);
  }
}
