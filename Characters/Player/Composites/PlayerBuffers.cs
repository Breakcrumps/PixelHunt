using System;
using System.Collections.Generic;
using Godot;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class PlayerBuffers : Node
{
  [Export] private int _longPressFrames = 10;
  
  private readonly Dictionary<string, GameTime> _bufferedButtons = new()
  {
    ["Rotate"] = GameTime.Zero,
    ["Pulse"] = GameTime.Zero,
    ["Stasis"] = GameTime.Zero
  };

  internal event Action<string>? ShortPress;

  internal bool ButtonLongPressed(string button)
  {
    if (!_bufferedButtons.TryGetValue(button, out GameTime bufferFrames))
      return false;

    return bufferFrames > GameTime.Frame * _longPressFrames;
  }

  public override void _Ready()
    => GlobalInstances.PlayerBuffers = this;

  public override void _PhysicsProcess(double delta)
  {
    foreach (var (button, bufferTime) in _bufferedButtons)
    {
      if (Input.IsActionPressed(button))
        _bufferedButtons[button] += GameTime.Frame;
      else
      {
        if (ButtonShortReleased(button))
          ShortPress?.Invoke(button);

        _bufferedButtons[button] = GameTime.Zero;
      }
    }
  }
  
  private bool ButtonShortReleased(string button)
  {
    if (!_bufferedButtons.TryGetValue(button, out GameTime bufferFrames))
      return false;

    return bufferFrames > GameTime.Zero && bufferFrames <= GameTime.Frame * _longPressFrames;
  }
}
