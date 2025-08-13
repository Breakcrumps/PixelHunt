using PixelHunt.Animation;
using PixelHunt.Characters.Enemy.States;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Characters.Player.MoveStrategies;

namespace PixelHunt.Static;

internal static class DebugFlags
{
  private const bool Debug = false;
  private const bool AnimatorDebug = false;
  private const bool PlayerAnimatorDebug = false;
  private const bool FreeStrategyDebug = true;
  private const bool CameraStateMachineDebug = false;
  private const bool FollowStateDebug = false;

  internal static bool GetDebugFlag(object caller) => caller switch
  {
    PlayerAnimator => PlayerAnimatorDebug,
    Animator => AnimatorDebug,
    FreeMoveStrategy => FreeStrategyDebug,
    CameraStateMachine => CameraStateMachineDebug,
    FollowState => FollowStateDebug,
    _ => Debug
  };
}
