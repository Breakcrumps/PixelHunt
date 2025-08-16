using System;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;

namespace PixelHunt.Static;

internal static class PulseFunctions
{
  private static FunctionComposer EnemyLevel1() => new(
    new QuadraticComponent { A = .08f },
    new LinearComponent { A = .05f, Start = 2 },
    new SineComponent
    {
      Amplitude = Maths.RandomInRange(from: .4f, to: .6f),
      Frequency = Maths.RandomInRange(from: .05f, to: .07f, precision: 2),
      Start = 30
    },
    new QuadraticNullifier { Start = 150, End = 180 },
    new EndComponent { Start = 180 }
  );

  internal static FunctionComposer EnemyFunction(int level) => level switch
  {
    1 => EnemyLevel1(),
    _ => throw new ArgumentException("Invalid pulse level for enemy pulse!", nameof(level))
  };

  internal static FunctionComposer Pebble() => new(
    new QuadraticComponent { A = .08f },
    new LinearComponent { A = .05f, Start = 2 },
    new SineComponent
    {
      Amplitude = Maths.RandomInRange(from: .7f, to: .9f),
      Frequency = Maths.RandomInRange(from: .07f, to: .09f, precision: 2),
      Start = 30
    },
    new QuadraticNullifier { Start = 150, End = 180 },
    new EndComponent { Start = 180 }
  );
}
