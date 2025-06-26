using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

[GlobalClass]
public partial class Animator : Node
{
  [Export] private AnimationPlayer? _animPlayer;
  [Export] private AnimationHelper? _animHelper;

  private readonly List<string> _uninterruptableAnims = ["Unsheathe"];

  public string AnimPrefix { private get; set; } = "";

  public void PlayAnimation(string animName, double blendTime = .15)
  {
    if (_animPlayer is null)
      return;

    animName = $"{AnimPrefix}{animName}";

    if (_animPlayer.CurrentAnimation == animName)
      return;

    if (_uninterruptableAnims.Contains(_animPlayer.CurrentAnimation))
      return;

    if (_animPlayer.HasAnimation(animName))
      _animPlayer.Play(animName, blendTime);

    HandleHelperAnim(animName);
  }

  private void HandleHelperAnim(string animName)
  {
    if (_animHelper is null)
      return;

    if (_animHelper.GetAnimationList().Contains(animName))
      _animHelper.Play(animName);
  }

  public void StopAnimation()
  {
    _animPlayer?.Stop();
    _animHelper?.Stop();
  }
}
