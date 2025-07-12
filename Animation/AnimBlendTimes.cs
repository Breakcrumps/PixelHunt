using System.Collections.Generic;
using GameSrc.Parents;
using GameSrc.Player;
using GameSrc.Enemy;
using System.Text.Json;
using System.IO;

namespace GameSrc.Animation;

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