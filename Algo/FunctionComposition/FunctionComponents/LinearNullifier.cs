using System;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents;

internal sealed class LinearNullifier : FunctionComponent
{
  internal required int End { private get; init; }

  private protected override float Algorithm(int t)
    => throw new AccessViolationException();

  internal override float Compute(int t)
    => StartValue * (1 - (float)(t - Start) / (End - Start));
}
