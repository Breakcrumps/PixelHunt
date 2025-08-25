using System;
using Godot;

namespace PixelHunt.Static;

internal static class Maths
{
  private const float Tolerance = 1e-15f;

  internal static float Round(this float operand, int digits)
    => MathF.Round(operand, digits);

  internal static float SinF(this float operand)
    => Mathf.Sin(operand);

  internal static bool IsRoughly(this float operand, float n, float tolerance = Tolerance)
    => Mathf.Abs(operand - n) < tolerance;

  internal static bool IsRoughlyZero(this float operand, float tolerance = Tolerance)
    => Mathf.Abs(operand - 0f) < tolerance;

  internal static bool IsRoughly(this Vector2 operand, Vector2 to, float tolerance = Tolerance)
    => operand.X.IsRoughly(to.X, tolerance) && operand.Y.IsRoughly(to.Y, tolerance);

  internal static bool IsRoughlyZero(this Vector2 operand, float tolerance = Tolerance)
    => operand.X.IsRoughlyZero(tolerance) && operand.Y.IsRoughlyZero(tolerance);

  internal static bool IsRoughlyZero(this Vector3 operand, float tolerance = Tolerance) => (
    operand.X.IsRoughlyZero(tolerance)
    && operand.Y.IsRoughlyZero(tolerance)
    && operand.Z.IsRoughlyZero(tolerance)
  );

  internal static bool BetweenRadians(this float operand, float left, float right, bool outer = false) => (
    outer
    ? left * Mathf.Pi >= operand || operand >= right * Mathf.Pi
    : left * Mathf.Pi <= operand && operand <= right * Mathf.Pi
  );

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

  internal static float LerpF(this float operand, float to, float weight)
    => Mathf.Lerp(from: operand, to: to, weight: weight);

  internal static float Abs(this float operand)
    => Mathf.Abs(operand);

  internal static bool IsRoughly(this Quaternion operand, Quaternion other, float tolerance = Tolerance) => (
    operand.X.IsRoughly(other.X, tolerance) && operand.Y.IsRoughly(other.Y, tolerance)
    && operand.Z.IsRoughly(other.Z, tolerance) && operand.W.IsRoughly(other.W, tolerance)
  );
}
