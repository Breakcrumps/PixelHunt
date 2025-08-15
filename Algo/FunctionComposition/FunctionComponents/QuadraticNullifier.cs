using System;
using PixelHunt.Static;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents;

internal sealed class QuadraticNullifier : FunctionComponent
{
  internal required int End { private get; init; }
  
  private protected override float Algorithm(int t)
    => throw new AccessViolationException();

  internal override float Compute(int t)
  {
    float a = -StartValue / (End - Start).Pow(2);

    return a * (t - Start).Pow(2) + StartValue;
  }
}
