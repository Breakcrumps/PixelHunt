namespace PixelHunt.Utils.FunctionComponents;

internal sealed class LinearComponent : FunctionComponent
{
  internal float A { private get; init; } = 1f;

  private protected override float Algorithm(int t)
    => A * t;
}
