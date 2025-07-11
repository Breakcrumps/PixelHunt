using Godot;

namespace GameSrc.Static;

internal static class InputHelper
{
  internal static Vector2 GetMovementDirection()
    => Input.GetVector(
      negativeX: "Left",
      positiveX: "Right",
      negativeY: "Down",
      positiveY: "Up"
    );
}