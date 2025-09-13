namespace PixelHunt.Algo.FunctionComposition;

internal abstract class FunctionComponent
{
  internal int Start { get; init; } = 0;
  /// <summary>
  /// Do NOT set yourself, FunctionComposer sets this automatically.
  /// </summary>
  internal float StartValue { get; set; } = 0f;

  /// <summary>
  /// This function is meant to simplify algorithms placed in <c>Algorithm()</c>, as the expression given below is normally enough to displace the graph.
  /// Override if the expression is easier to write / understand with manual / special displacement. If this is done, <c>throw InvalidAccess</c> in <c>Algorithm()</c>.
  /// </summary>
  /// <param name="t"> Plain independent parameter. </param>
  /// <returns></returns>
  internal virtual float Compute(int t)
    => Algorithm(t - Start) + StartValue;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="t"> Displaced independent parameter. </param>
  /// <returns></returns>
  private protected abstract float Algorithm(int t);
}
