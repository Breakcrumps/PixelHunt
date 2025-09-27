using PixelHunt.Static;
using Godot;

namespace PixelHunt.Animation;

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

  internal void Unsheathe()
  {
    if (Character is null)
      return;

    if (AnimPrefix == "Unsheathed")
      return;

    PlayAnimation(
      !InputHelper.IsMovementInput()
      ? "Unsheathe"
      : "RunUnsheathe",
      bypass: true
    );

    CanProcessRequests = false;
  }

  internal void Sheathe()
  {
    if (Character is null)
      return;

    if (AnimPrefix != "Unsheathed")
      return;

    PlayAnimation(
      !InputHelper.IsMovementInput()
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

    switch (CurrentAnim)
    {
      case "RunUnsheathe" when !InputHelper.IsMovementInput():
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
      case "Unsheathe" when InputHelper.IsMovementInput():
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
