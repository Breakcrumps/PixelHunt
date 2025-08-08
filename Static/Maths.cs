using Godot;

namespace PixelHunt.Static;

internal static class Maths
{
  private const float Tolerance = 1e-15f;

  internal static bool IsRoughly(this float left, float n)
    => Mathf.Abs(left - n) < Tolerance;
}
