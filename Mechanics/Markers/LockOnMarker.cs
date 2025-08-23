using Godot;

namespace PixelHunt.Mechanics.Markers;

[GlobalClass]
internal sealed partial class LockOnMarker : Node3D
{
  [Export] private Sprite3D? _sprite;
  [Export] private AnimationPlayer? _animPlayer;

  public override void _Ready()
  {
    if (_sprite is null)
      return;
    
    _sprite.Visible = false;
  }
  
  internal void ShowMarker()
  {
    if (_animPlayer is null)
      return;

    if (_sprite is null)
      return;

    _animPlayer.Play("Spin");
    _sprite.Visible = true;
  }

  internal void HideMarker()
  {
    if (_animPlayer is null)
      return;

    if (_sprite is null)
      return;

    _animPlayer.Stop();
    _sprite.Visible = false;
  }
}
