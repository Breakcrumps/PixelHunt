using System;
using System.Linq;
using Godot;

[GlobalClass]
public partial class Animator : Node
{
  [Export] private AnimationPlayer? _animPlayer;
  [Export] private AnimationHelper? _animHelper;

  public void PlayAnimation(string animName, double blendTime = .15)
  {
    if (_animPlayer is null)
      return;

    if (_animPlayer.CurrentAnimation == animName)
      return;

    if (_animPlayer.HasAnimation(animName))
      _animPlayer.Play(animName, blendTime);

    HandleHelperAnim(animName);
  }

  private void HandleHelperAnim(string animName)
  {
    if (_animHelper is null)
      return;

    _animHelper.Stop();

    if (_animHelper.GetAnimationList().Contains(animName))
      _animHelper.Play(animName);
  }
}
