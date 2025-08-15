namespace PixelHunt.Algo.FunctionComposition;

internal abstract class FunctionComponent
{
  internal int Start { get; init; } = 0;
  /// <summary> Do NOT set yourself, FunctionComposer sets this automatically. </summary>
  internal float StartValue { get; set; } = 0f;

  internal virtual float Compute(int t)
    => Algorithm(t - Start) + StartValue;

  private protected abstract float Algorithm(int t);
}
