using System;

public static class AnimationBus
{
  public static event Action<string, float>? AnimPlay;

  public static void PlayAnim(string animName, float blendTime) => AnimPlay?.Invoke(animName, blendTime);
}