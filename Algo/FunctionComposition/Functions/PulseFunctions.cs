using System;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Nullifiers;
using PixelHunt.Static;

namespace PixelHunt.Algo.FunctionComposition.Functions;

internal enum RubbishType { Generic, Wall }

internal static class PulseFunctions
{
  private static FunctionComposer GenerateEnemyLevel1() => new(
    new QuadraticComponent { A = Maths.RandomInRange(.07f, .1f, 2) },
    new LinearComponent { A = .05f, Start = 2 },
    new SineComponent
    {
      Amplitude = Maths.RandomInRange(from: .2f, to: .4f),
      Frequency = Maths.RandomInRange(from: .05f, to: .07f, precision: 2),
      Start = 30
    },
    new QuadraticNullifier { Start = 150, End = 180 },
    new EndComponent { Start = 180 }
  );

  internal static FunctionComposer GenerateEnemyFunction(int level) => level switch
  {
    1 => GenerateEnemyLevel1(),
    _ => throw new ArgumentException("Invalid pulse level for enemy pulse!", nameof(level))
  };

  internal static FunctionComposer GenerateRubbishFunction(RubbishType rubbleType) => rubbleType switch
  {
    RubbishType.Wall => GenerateWallFunction(),
    _ => GenerateGenericRubbishFunction()
  };

  private static FunctionComposer GenerateGenericRubbishFunction() => new(
    new QuadraticComponent { A = Maths.RandomInRange(.07f, .1f, 2) },
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

  private static FunctionComposer GenerateWallFunction() => new(
    new QuadraticComponent { A = Maths.RandomInRange(.07f, .1f, 2) },
    new LinearComponent { A = .07f, Start = 2 },
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
