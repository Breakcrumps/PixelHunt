using System.Collections.Generic;
using PixelHunt.Mechanics.Pulse;
using PixelHunt.Mechanics.Stasis;

namespace PixelHunt.Static;

internal static class NodeGroups
{
  internal static List<PulseSource> PulseSources { get; } = [];
  internal static List<StasisSource> StasisSources { get; } = [];
}
