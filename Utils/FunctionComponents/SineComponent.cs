using System;

namespace PixelHunt.Utils.FunctionComponents;

internal sealed class SineComponent : FunctionComponent
{
  internal float Amplitude { private get; init; } = 1f;
  internal float Frequency { private get; init; } = 1f;

  private protected override float Algorithm(int t)
    => Amplitude * MathF.Sin(Frequency * t);
}
