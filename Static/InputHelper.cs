using Godot;

namespace PixelHunt.Static;

internal static class InputHelper
{
  internal static bool IsMovementInput()
    => GetMovementDirection() != Vector2.Zero;

  internal static Vector2 GetMovementDirection()
    => Input.GetVector(
      negativeX: "Left", positiveX: "Right",
      negativeY: "Up", positiveY: "Down"
    );
}
