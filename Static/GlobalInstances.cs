using Godot;
using PixelHunt.Characters.Player;

namespace PixelHunt.Static;

internal static class GlobalInstances
{
  internal static PlayerChar? PlayerChar { get; set; }
  internal static Camera3D? PlayerCamera { get; set; }
}
