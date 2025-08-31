using PixelHunt.Static;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents.Nullifiers;

internal sealed class QuadraticNullifier : FunctionComponent
{
  internal required int End { private get; init; }

  private protected override float Algorithm(int t)
  {
    float a = -StartValue / (End - Start).Pow(2);

    return a * t.Pow(2); 
  }
}
