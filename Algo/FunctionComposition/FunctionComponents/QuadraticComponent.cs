namespace PixelHunt.Algo.FunctionComposition.FunctionComponents;

internal sealed class QuadraticComponent : FunctionComponent
{
  internal float A { private get; init; } = 1f;
  internal float B { private get; init; } = 0f;

  private protected override float Algorithm(int t)
    => t * (A * t + B);
}
