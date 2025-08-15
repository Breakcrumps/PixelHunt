using PixelHunt.Utils;
using PixelHunt.Utils.FunctionComponents;

namespace PixelHunt.Static;

internal static class PulseFunctions
{
  internal static FunctionComposer Default => new(
    new QuadraticComponent { A = .08f, B = .1f },
    new LinearComponent { A = .2f, Start = 5 },
    new SineComponent { Amplitude = .5f, Frequency = .08f, Start = 10 },
    new EndComponent { Start = 150 }
  );
}
