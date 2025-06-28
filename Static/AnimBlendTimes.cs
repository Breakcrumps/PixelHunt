using BlendTimes =
  System.Collections.Generic.Dictionary<
    string,
    System.Collections.Generic.Dictionary<string, double>
  >;

public static class AnimBlendTimes
{
  public static double DefaultBlendTime => .15;

  private static readonly BlendTimes _playerBlendTimes = new()
  {
    ["*"] = new()
    {
      ["*"] = .15,
      ["Fall"] = .21
    },
    ["Unsheathe"] = new()
    {
      ["UnsheathedIdle"] = .0
    }
  };

  private static readonly BlendTimes _enemyBlendTimes = new()
  {
    ["*"] = new()
    {
      ["*"] = .2
    }
  };

  public static BlendTimes GetBlendTimes(Character character) => character switch
  {
    Player => _playerBlendTimes,
    Enemy => _enemyBlendTimes,
    _ => []
  };
}