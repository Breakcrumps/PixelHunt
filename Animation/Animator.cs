using System.Text;
using PixelHunt.Characters;
using PixelHunt.Static;
using Godot;

namespace PixelHunt.Animation;

[GlobalClass]
internal partial class Animator : Node
{
  [Export] internal bool CanProcessRequests { private get; set; } = true;

  [Export] private protected AnimationPlayer? AnimPlayer { get; private set; }
  [Export] private AnimationHelper? _animHelper;

  [Export] private protected Character? Character { get; private set; }

  [Export] private protected string AnimPrefix { get; set; } = "";

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

    if (!noPrefix)
      animName = $"{AnimPrefix}{animName}";

    HandleBaseAnimation(animName, startPos);

    HandleHelperAnim(animName, startPos);
  }

  private void HandleBaseAnimation(string animName, double startPos = .0)
  {
    if (AnimPlayer is null)
      return;

    if (AnimPlayer.CurrentAnimation == animName)
      return;

    if (!AnimPlayer.HasAnimation(animName))
    {
      GD.Print($"No animation called {animName} found.");
      return;
    }

    double blendTime = DetermineBlendTime(CurrentAnim, animName);

    AnimPlayer.Play(animName, blendTime);
    AnimPlayer.Seek(startPos);

    CurrentAnim = animName;
  }

  private void HandleHelperAnim(string animName, double startPos = .0)
  {
    if (_animHelper is null)
      return;

    if (_animHelper.CurrentAnimation == animName)
      return;

    if (!_animHelper.HasAnimation(animName))
      return;

    _animHelper.Play(animName);
    _animHelper.Seek(startPos);
  }

  /// <summary>
  /// !!! NOT RECOMENDED FOR USE !!! Bypasses a lot of logic associated with Animator.PlayAnimation().
  /// </summary>
  /// <param name="animName"></param>
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

    var charBlendTimes = AnimBlendTimes.GetBlendTimes(Character);

    if (!charBlendTimes.TryGetValue(from, out var fromBlendTimes))
      fromBlendTimes = charBlendTimes["*"];

    if (!fromBlendTimes.TryGetValue(to, out double blendTime))
      blendTime = charBlendTimes["*"]["*"];

    return blendTime;
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
