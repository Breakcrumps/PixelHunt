using System.Linq;
using Godot;
using GameSrc.Parents;
using GameSrc.Enemy;

[GlobalClass]
internal sealed partial class Animator : Node
{
  [Export] private bool CanProcessRequests { get; set; } = true;

  [Export] private AnimationPlayer? _animPlayer;
  [Export] private AnimationHelper? _animHelper;

  [Export] private Character? _character;

  internal string AnimPrefix { private get; set; } = "";

  private Vector2 _horizontalVelocity;

  private string _currentAnim = "";

  public override void _Ready()
  {
    if (Flags.Debug)
      DEBUG_DumpBlendTimes();
  }
  
  internal void PlayAnimation(
    string animName,
    double startPos = .0,
    bool bypass = false
  )
  {
    if (!CanProcessRequests && !bypass)
      return;

    if (_animPlayer is null)
      return;

    animName = $"{AnimPrefix}{animName}";

    if (_animPlayer.CurrentAnimation == animName)
      return;

    if (_animPlayer.HasAnimation(animName))
    {
      if (Flags.Debug)
        GD.Print($"PlayAnimation shipped {animName} to _animPlayer on {_character!.Name}!");

      _animPlayer.Seek(startPos);

      double blendTime = DetermineBlendTime(_currentAnim, animName);

      _animPlayer.Play(animName, blendTime);

      if (Flags.Debug && _character is EnemyChar)
        GD.Print($"Blend time was {blendTime} here!");

      _currentAnim = animName;
    }

    HandleHelperAnim(animName, startPos);
  }

  private void HandleHelperAnim(string animName, double startPos = .0)
  {
    if (_animHelper is null)
      return;

    if (!_animHelper.GetAnimationList().Contains(animName))
      return;
      
    if (Flags.Debug)
      GD.Print($"HandleHelperAnim shipped {animName} to _animHelper on {_character!.Name}!");

    _animHelper.Seek(startPos);
    _animHelper.Play(animName);
  }

  internal void StopAnimation()
  {
    if (!CanProcessRequests)
      return;

    _animPlayer?.Stop();
    _animHelper?.Stop();
  }

  public override void _Process(double delta)
  {
    if (_animPlayer?.CurrentAnimation is "Unsheathe" or "RunUnsheathe")
      ContinueUnsheathe();
  }

  internal void Unsheathe()
  {
    if (_character is null)
      return;

    _horizontalVelocity = new Vector2(_character.Velocity.X, _character.Velocity.Z);

    if (_horizontalVelocity == Vector2.Zero)
      PlayAnimation("Unsheathe");
    else
      PlayAnimation("RunUnsheathe");

    CanProcessRequests = false;

    if (Flags.Debug)
      DEBUG_NotifyRequestClose();
  }

  private void ContinueUnsheathe()
  {
    if (_animPlayer is null)
      return;

    double currentTime = _animPlayer!.CurrentAnimationPosition;

    if (_horizontalVelocity == Vector2.Zero && _currentAnim == "RunUnsheathe")
      PlayAnimation("Unsheathe", startPos: currentTime, bypass: true);
    else if (_horizontalVelocity != Vector2.Zero && _currentAnim == "Unsheathe")
      PlayAnimation("RunUnsheathe", startPos: currentTime, bypass: true);
  }

  private double DetermineBlendTime(string from, string to)
  {
    if (_character is null)
      return AnimBlendTimes.DefaultBlendTime;

    if (!AnimBlendTimes.GetBlendTimes(_character).TryGetValue(from, out var fromBlendTimes))
      fromBlendTimes = AnimBlendTimes.GetBlendTimes(_character)["*"];

    if (!fromBlendTimes.TryGetValue(to, out double blendTime))
      blendTime = AnimBlendTimes.GetBlendTimes(_character)["*"]["*"];

    return blendTime;
  }

  internal void DEBUG_NotifyRequestClose()
  {
    GD.Print($"Request closed on {_character!.Name}'s Animator.");
  }
  internal void DEBUG_NotifyRequestOpen()
  {
    if (!Flags.Debug)
      return;

    GD.Print($"Request opened on {_character!.Name}'s Animator.");
  }

  private void DEBUG_DumpBlendTimes()
  {
    if (_character is null)
      return;

    foreach (var (from, blendTimes) in AnimBlendTimes.GetBlendTimes(_character))
    {
      GD.Print($"{from}:");

      foreach (var (to, blendTime) in blendTimes)
      {
        GD.Print($"\t{to}: {blendTime}");
      }
    }
  }
}
