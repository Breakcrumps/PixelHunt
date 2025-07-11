using System.Collections.Generic;
using GameSrc.Parents;
using GameSrc.Player;
using GameSrc.Enemy;

namespace GameSrc.Static;

using BlendTimes =
  Dictionary<
    string,
    Dictionary<string, double>
  >;

internal static class AnimBlendTimes
{
  internal static double DefaultBlendTime => .15;

  private static readonly BlendTimes PlayerBlendTimes = new()
  {
    ["*"] = new Dictionary<string, double>
    {
      ["*"] = .15,
      ["Fall"] = .21
    },
    ["Unsheathe"] = new Dictionary<string, double>
    {
      ["UnsheathedIdle"] = .0
    },
    ["RunUnsheathe"] = new Dictionary<string, double>
    {
      ["UnsheathedRun"] = .0
    }
  };

  private static readonly BlendTimes EnemyBlendTimes = new()
  {
    ["*"] = new Dictionary<string, double>
    {
      ["*"] = .2
    }
  };

  internal static BlendTimes GetBlendTimes(Character character) => character switch
  {
    PlayerChar => PlayerBlendTimes,
    EnemyChar => EnemyBlendTimes,
    _ => []
  };
}