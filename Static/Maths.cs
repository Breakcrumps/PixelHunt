using Godot;

internal static class Maths
{
  private const float Tolerance = 1e-15f;

  internal static bool IsRoughly(this float left, float n)
  {
    return Mathf.Abs(left - n) < Tolerance;
  }
}