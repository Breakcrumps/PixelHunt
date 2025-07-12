using Godot;
using GameSrc.Parents;
using GameSrc.Enemy;
using GameSrc.Static;

namespace GameSrc.Animation;

[GlobalClass]
internal partial class Animator : Node
{
  [Export] private protected bool CanProcessRequests { get; set; } = true;

  [Export] private protected AnimationPlayer? AnimPlayer { get; private set; }
  [Export] private AnimationHelper? _animHelper;

  [Export] private protected Character? Character { get; private set; }

  private protected string AnimPrefix { get; set; } = "";

  private protected string CurrentAnim { get; private set; } = "";

  public override void _Ready()
  {
    if (DebugFlags.GetDebugFlag(this))
      DEBUG_DumpBlendTimes();
  }
  
  internal void PlayAnimation(
    string animName,
    double startPos = .0,
    bool bypass = false,
    bool noPrefix = false
  )
  {
    if (!CanProcessRequests && !bypass)
      return;

    if (AnimPlayer is null)
      return;

    animName = noPrefix ? $"{animName}" : $"{AnimPrefix}{animName}";

    HandleBaseAnimation(animName, startPos);

    HandleHelperAnim(animName, startPos);
  }

  private void HandleBaseAnimation(string animName, double startPos = .0)
  {
    if (AnimPlayer!.CurrentAnimation == animName)
      return;

    if (!AnimPlayer.HasAnimation(animName))
      return;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"PlayAnimation shipped {animName} to _animPlayer on {Character!.Name}!");

    double blendTime = DetermineBlendTime(CurrentAnim, animName);

    AnimPlayer.Play(animName, blendTime);
    AnimPlayer.Seek(startPos);

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"Blend time was {blendTime} here!");

    CurrentAnim = animName;
  }

  private void HandleHelperAnim(string animName, double startPos = .0)
  {
    if (_animHelper is null)
      return;

    if (!_animHelper.HasAnimation(animName))
      return;

    if (DebugFlags.GetDebugFlag(this))
      GD.Print($"HandleHelperAnim shipped {animName} to _animHelper on {Character!.Name}!");

    _animHelper.Play(animName);
    _animHelper.Seek(startPos);
  }

  internal void QueueAnimation(string animName)
  {
    animName = $"{AnimPrefix}{animName}";

    if (
      AnimPlayer is not null
      && AnimPlayer!.HasAnimation(animName)
    )
      AnimPlayer.Queue(animName);

    if (
      _animHelper is not null
      && _animHelper.HasAnimation(animName)
    )
      _animHelper.Queue(animName);
  }

  internal void StopAnimation(bool bypass = false)
  {
    if (!CanProcessRequests && !bypass)
      return;

    AnimPlayer?.Stop();
    _animHelper?.Stop();
  }

  private double DetermineBlendTime(string from, string to)
  {
    if (Character is null)
      return AnimBlendTimes.DefaultBlendTime;

    if (!AnimBlendTimes.GetBlendTimes(Character).TryGetValue(from, out var fromBlendTimes))
      fromBlendTimes = AnimBlendTimes.GetBlendTimes(Character)["*"];

    if (!fromBlendTimes.TryGetValue(to, out double blendTime))
      blendTime = AnimBlendTimes.GetBlendTimes(Character)["*"]["*"];

    return blendTime;
  }

  internal void DEBUG_NotifyRequestClose()
  {
    GD.Print($"Request closed on {Character!.Name}'s Animator.");
  }
  internal void DEBUG_NotifyRequestOpen()
  {
    if (!DebugFlags.GetDebugFlag(this))
      return;

    GD.Print($"Request opened on {Character!.Name}'s Animator.");
  }

  private void DEBUG_DumpBlendTimes()
  {
    if (Character is null)
      return;

    foreach (var (from, blendTimes) in AnimBlendTimes.GetBlendTimes(Character))
    {
      GD.Print($"{from}:");

      foreach (var (to, blendTime) in blendTimes)
      {
        GD.Print($"\t{to}: {blendTime}");
      }
    }
  }
}
