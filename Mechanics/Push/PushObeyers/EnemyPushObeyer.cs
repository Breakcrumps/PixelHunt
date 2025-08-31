using Godot;
using PixelHunt.Characters.Enemy;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Enemy.States;

namespace PixelHunt.Mechanics.Push.PushObeyers;

[GlobalClass]
internal sealed partial class EnemyPushObeyer : PushObeyer
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private PushedState? _pushedState;
  
  private protected override void ObeyPush(PushParams pushParams)
  {
    if (StasisObeyer is null || StasisObeyer.InStasis)
      return;

    if (_enemyChar is null)
      return;

    if (_pushedState is null)
      return;

    if (_enemyChar.GlobalPosition.DistanceTo(pushParams.Actor.GlobalPosition) > 20f)
      return;

    _pushedState.Actor = pushParams.Actor;
    _pushedState.InitialVelocity = pushParams.Direction * _effectStrength;

    _stateMachine?.Transition("PushedState");
  }
}
