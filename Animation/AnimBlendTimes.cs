using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GameSrc.Characters;
using GameSrc.Characters.Enemy;
using GameSrc.Characters.Player;

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