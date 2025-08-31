using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Modifiers;

namespace PixelHunt.Algo.FunctionComposition.Functions;

internal static class StasisFunctions
{
  internal static readonly FunctionComposer StasisWiggle = new(
    new SineComponent { Shift = 3, Damper = new DamperModifier { Severity = 2f, Sharpness = .1f }, FrequencyFunc = t => t },
    new EndComponent { Start = 100 }
  );
}
