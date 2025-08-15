using System;

namespace PixelHunt.Utils.FunctionComponents;

internal sealed class EndComponent : FunctionComponent
{
  private protected override float Algorithm(int t)
    => throw new AccessViolationException("Tried to compute endpoint component!");
}
