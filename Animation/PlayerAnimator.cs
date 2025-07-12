using GameSrc.Static;
using Godot;

namespace GameSrc.Animation;

[GlobalClass]
internal sealed partial class PlayerAnimator : Animator
{
  public override void _Process(double delta)
  {
    switch (AnimPlayer?.CurrentAnimation)
    {
      case "Unsheathe" or "RunUnsheathe":
        ContinueUnsheathe();
        break;
    }
  }

  internal void FlipUnsheathe()
  {
    if (AnimPrefix == "Unsheathed")
      Sheathe();
    else
      Unsheathe();
  }

  private void Unsheathe()
  {
    if (Character is null)
      return;

    Vector2 inputDirection = InputHelper.GetMovementDirection();

    PlayAnimation(
      inputDirection == Vector2.Zero
      ? "Unsheathe"
      : "RunUnsheathe"
    );

    CanProcessRequests = false;

    AnimPrefix = "Unsheathed";

    if (DebugFlags.GetDebugFlag(this))
      DEBUG_NotifyRequestClose();
  }

  private void Sheathe() => AnimPrefix = "";

  private void ContinueUnsheathe()
  {
    if (AnimPlayer is null)
      return;

    double currentTime = AnimPlayer!.CurrentAnimationPosition;
    Vector2 inputDirection = InputHelper.GetMovementDirection();

    switch (CurrentAnim)
    {
      case "RunUnsheathe" when inputDirection == Vector2.Zero:
        if (currentTime > .4)
          return;

        PlayAnimation(
          "Unsheathe",
          startPos: currentTime + .35,
          bypass: true,
          noPrefix: true
        );
        break;
      case "Unsheathe" when inputDirection != Vector2.Zero:
        PlayAnimation(
          "RunUnsheathe",
          startPos: currentTime - .3,
          bypass: true,
          noPrefix: true
        );
        break;
    }
  }

  internal void Run()
  {
    if (
      CurrentAnim == $"{AnimPrefix}RunStart"
      || CurrentAnim == $"{AnimPrefix}Run"
      || CurrentAnim == $"RunUnsheathe"
    )
      return;
      
    if (
      AnimPlayer!.HasAnimation($"{AnimPrefix}RunStart")
      && (CurrentAnim != $"{AnimPrefix}Fall" || CurrentAnim != $"{AnimPrefix}Rise")
    )
      PlayAnimation("RunStart");
    else
      PlayAnimation("Run");
  }

  internal void StopOrIdle()
  {
    switch (AnimPlayer?.CurrentAnimation)
    {
      case var run when run == $"{AnimPrefix}Run":
        PlayAnimation("RunEnd");
        break;
      case "RunUnsheathe":
        PlayAnimation("RunEnd", bypass: true);
        CanProcessRequests = true;
        break;
      case var notRunEnd when notRunEnd != $"{AnimPrefix}RunEnd":
        PlayAnimation("Idle");
        break;
    }
  }
} 