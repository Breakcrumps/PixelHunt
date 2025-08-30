using Godot;

namespace PixelHunt.Algo.FunctionComposition.FunctionComponents.Modifiers;

internal sealed class DamperModifier : FunctionComponent
{
  /// <summary>
  /// Hard to name. Less than one means damping leftwards, and vice versa. The more the distance from 1f, the more extreme the undamped part.
  /// </summary>
  internal float Severity { private get; set; } = 1f;

  /// <summary>
  /// A value lower than 1f advised.
  /// </summary>
  internal float Sharpness { private get; set; } = 1f;

  private protected override float Algorithm(int t)
    => Mathf.Pow(Severity, -Sharpness * t);
}
