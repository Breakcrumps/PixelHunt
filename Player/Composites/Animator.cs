using System;
using System.Linq;
using Godot;

[GlobalClass]
public partial class Animator : Node
{
  [Export] public Model? Model { get; private set; }

  public void PlayAnimation(string animName, double blendTime = .15)
  {
    if (Model is null)
      return;

    if (Model.AnimationPlayer!.CurrentAnimation == animName)
      return;

    Model.AnimationPlayer?.Play(animName, blendTime);

    HandleHelperAnim(animName);
  }

  private void HandleHelperAnim(string animName)
  {
    if (Model is null)
      return;

    if (Model.AnimationHelper is null)
      return;

    Model.AnimationHelper.Stop();

    if (Model.AnimationHelper.GetAnimationList().Contains(animName))
      Model?.AnimationHelper?.Play(animName);
  }
}
