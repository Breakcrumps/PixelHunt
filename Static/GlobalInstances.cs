using Godot;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Player.Composites;

namespace PixelHunt.Static;

internal static class GlobalInstances
{
  internal static PlayerChar? PlayerChar { get; set; }
  internal static Camera3D? PlayerCamera { get; set; }
  internal static PlayerBuffers? PlayerBuffers { get; set; }
}
