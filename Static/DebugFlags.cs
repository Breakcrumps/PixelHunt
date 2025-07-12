using GameSrc.Animation;
using GameSrc.Characters.Player.MoveStrategies;

namespace GameSrc.Static;

internal static class DebugFlags
{
  private const bool Debug = false;
  private const bool AnimatorDebug = false;
  private const bool PlayerAnimatorDebug = false;
  private const bool FreeStrategyDebug = false;

  internal static bool GetDebugFlag(object caller) => caller switch
  {
    PlayerAnimator => PlayerAnimatorDebug,
    Animator => AnimatorDebug,
    FreeMoveStrategy => FreeStrategyDebug,
    _ => Debug
  };
}
