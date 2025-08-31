using Godot;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents.Nullifiers;

internal sealed class SineNullifier : FunctionComponent
{
  internal required int End { private get; init; }
  
  internal float Amplitude { private get; init; } = 1f;
  internal float Phase { private get; init; } = 0f;

  internal int Shift { private get; init; } = 0;

  private protected override float Algorithm(int t)
  {
    float frequency = Mathf.Pi / (End - Start);

    return Amplitude * Mathf.Sin(frequency * (t - Shift) + Phase);
  }
}
