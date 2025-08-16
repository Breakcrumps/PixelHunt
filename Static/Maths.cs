using System;
using Godot;

namespace PixelHunt.Static;

internal static class Maths
{
  private const float Tolerance = 1e-15f;

  internal static bool IsRoughly(this float left, float n)
    => Mathf.Abs(left - n) < Tolerance;

  internal static int Pow(this int operand, int power)
  {
    if (power < 1)
      throw new ArgumentException("Wrong power for int.Pow!", nameof(power));

    if (power == 1)
      return operand;

    if (power == 2)
      return operand * operand;

    while (power != 1)
    {
      operand *= operand;
      power--;
    }

    return operand;
  }

  internal static float RandomInRange(float from, float to, int precision = 1)
    => from + MathF.Round(Random.Shared.NextSingle() * (to - from), precision);
}
