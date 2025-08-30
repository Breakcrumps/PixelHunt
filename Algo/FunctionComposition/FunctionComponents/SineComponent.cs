using System;
using Godot;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Modifiers;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents;

/// <summary>
/// See: https://www.desmos.com/calculator/pb6wewrr37
/// </summary>
internal sealed class SineComponent : FunctionComponent
{
  internal float Amplitude { private get; init; } = 1f;
  internal float Phase { private get; init; } = 0f;

  /// <summary>
  /// Overriden by <c>FrequencyFunc</c>, if any.
  /// </summary>
  internal float Frequency { private get; init; } = 1f;

  /// <summary>
  /// Overrides <c>Frequency</c> if existent.
  /// </summary>
  internal Func<int, float>? FrequencyFunc { private get; init; }

  internal int Shift { private get; init; } = 0;

  internal DamperModifier? Damper { private get; init; }

  private protected override float Algorithm(int t)
  {
    float frequency = FrequencyFunc is null ? Frequency : FrequencyFunc(t);
    float sine = Mathf.Sin(frequency * (t - Shift) + Phase);
    float damper = Damper?.Compute(t - Shift) ?? 1f;

    return Amplitude * sine * damper;
  }
}
