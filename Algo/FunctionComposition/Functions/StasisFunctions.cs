using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Modifiers;
using PixelHunt.Static;

namespace PixelHunt.Algo.FunctionComposition.Functions;

internal static class StasisFunctions
{
  internal static int[] WiggleAxes => [0, 2];
  
  internal static FunctionComposer GenerateStasisWiggle() => new(
    new SineComponent 
    {
      Shift = 3,
      Damper = new DamperModifier { Severity = 2f, Sharpness = .1f },
      FrequencyFunc = t => Maths.RandomInRange(.7f, 1.1f) * t
    },
    new EndComponent { Start = 100 }
  );
}
