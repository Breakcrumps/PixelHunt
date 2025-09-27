using System;
using Godot;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class PlayerBuffers : Node
{
  internal GameTime RotateButtonAccumulator { get; private set; }

  internal event Action? RotateShortPress;
  internal bool RotateLongPressed()
    => RotateButtonAccumulator > GameTime.Frame * 15;
  private bool RotateShortReleased() => (
    RotateButtonAccumulator > GameTime.Zero
    && RotateButtonAccumulator <= GameTime.Frame * 15
  );

  public override void _Ready()
    => GlobalInstances.PlayerBuffers = this;

  public override void _PhysicsProcess(double delta)
    => HandleRotate();

  private void HandleRotate()
  {
    if (Input.IsActionPressed("Rotate"))
      RotateButtonAccumulator += GameTime.Frame;
    else
    {
      if (RotateShortReleased())
        RotateShortPress?.Invoke();
      
      RotateButtonAccumulator = GameTime.Zero;
    }
  }
}
