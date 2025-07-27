using GameSrc.Static;
using Godot;

namespace GameSrc.Animation;

[GlobalClass]
internal sealed partial class PlayerAnimator : Animator
{
  public override void _Process(double delta)
  {
    if (AnimPlayer?.CurrentAnimation is "Unsheathe" or "RunUnsheathe")
      ContinueUnsheathe();
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
      : "RunUnsheathe",
      bypass: true
    );

    CanProcessRequests = false;
  }

  private void Sheathe()
  {
    if (Character is null)
      return;

    Vector2 inputDirection = InputHelper.GetMovementDirection();

    PlayAnimation(
      inputDirection == Vector2.Zero
      ? "Sheathe"
      : "RunSheathe",
      noPrefix: true,
      bypass: true
    );

    CanProcessRequests = false;
  }

  private void ContinueUnsheathe()
  {
    if (AnimPlayer is null)
      return;

    double currentTime = AnimPlayer.CurrentAnimationPosition;
    Vector2 inputDirection = InputHelper.GetMovementDirection();

    switch (CurrentAnim)
    {
      case "RunUnsheathe" when inputDirection == Vector2.Zero:
        if (currentTime > .4)
        {
          CanProcessRequests = true;
          return;
        }

        PlayAnimation(
          "Unsheathe",
          startPos: currentTime + .35,
          bypass: true,
          noPrefix: true
        );
        break;
      case "Unsheathe" when inputDirection != Vector2.Zero:
        if (currentTime > .7)
        {
          CanProcessRequests = true;
          return;
        }

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
      CurrentAnim == $"{AnimPrefix}Run"
      || CurrentAnim == "RunUnsheathe"
    )
      return;

    PlayAnimation("Run");
  }

  internal void StopOrIdle()
  {
    switch (AnimPlayer?.CurrentAnimation)
    {
      case var run when run == $"{AnimPrefix}Run":
        PlayAnimation("RunEnd");
        break;
      case "RunUnsheathe" or "RunSheathe":
        CanProcessRequests = true;
        PlayAnimation("RunEnd");
        break;
      case var notRunEnd when notRunEnd != $"{AnimPrefix}RunEnd":
        PlayAnimation("Idle");
        break;
    }
  }
} 
