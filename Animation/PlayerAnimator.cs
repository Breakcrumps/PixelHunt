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

    if (Flags.Debug)
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
          startPos: currentTime - .22,
          bypass: true,
          noPrefix: true
        );
        break;
    }
  }
} 