using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PixelHunt.Characters;
using PixelHunt.Characters.Enemy;
using PixelHunt.Characters.Player;

namespace PixelHunt.Animation;

using BlendTimes =
  Dictionary<
    string,
    Dictionary<string, double>
  >;

internal static class AnimBlendTimes
{
  internal static double DefaultBlendTime => .15;

  private static readonly BlendTimes? PlayerBlendTimes = ReadBlendTimes("PlayerBlendTimes");

  private static readonly BlendTimes? EnemyBlendTimes = ReadBlendTimes("EnemyBlendTimes");

  internal static BlendTimes GetBlendTimes(Character character) => character switch
  {
    PlayerChar => PlayerBlendTimes ?? [],
    EnemyChar => EnemyBlendTimes ?? [],
    _ => []
  };

  private static BlendTimes? ReadBlendTimes(string filename)
    => JsonSerializer.Deserialize<BlendTimes>(
      json: File.ReadAllText(@$"Animation\BlendTimes\{filename}.json")
    );
}
