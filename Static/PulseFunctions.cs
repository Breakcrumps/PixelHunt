using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;

namespace PixelHunt.Static;

internal static class PulseFunctions
{
  internal static FunctionComposer Default => new(
    new QuadraticComponent { A = .08f, B = .1f },
    new LinearComponent { A = .05f, Start = 2 },
    new SineComponent { Amplitude = .5f, Frequency = .06f, Start = 30 },
    new QuadraticNullifier { Start = 150, End = 180 },
    new EndComponent { Start = 180 }
  );
}
