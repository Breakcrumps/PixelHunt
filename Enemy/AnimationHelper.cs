using System;
using Godot;

public partial class AnimationHelper : AnimationPlayer
{
  [Export] public float Speed { get; set; }

  public event Action? HitboxOn;
  public event Action? HitboxOff;

  private void EnableHitbox() => HitboxOn?.Invoke();
  private void DisableHitbox() => HitboxOff?.Invoke();
}
