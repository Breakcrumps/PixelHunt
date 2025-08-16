using Godot;
using PixelHunt.Characters.Enemy;
using PixelHunt.Characters.Enemy.Composites;
using PixelHunt.Characters.Enemy.States;

namespace PixelHunt.Mechanics.Pulse.PulseObeyers;

[GlobalClass]
internal sealed partial class EnemyPulseObeyer : PulseObeyer
{
  [Export] private EnemyChar? _enemyChar;
  [Export] private EnemyStateMachine? _stateMachine;
  [Export] private PulseState? _pulseState;
  
  private protected override void ObeyPulse(PulseParams pulseParams)
  {
    if (_enemyChar is null)
      return;
    
    if (_pulseState is null)
      return;

    Vector3 distance = pulseParams.Actor.GlobalPosition - _enemyChar.GlobalPosition;

    if (distance.Length() > pulseParams.Radius)
      return;

    _pulseState.PulseParams = pulseParams;    
    _stateMachine?.Transition("PulseState");
  }
}
