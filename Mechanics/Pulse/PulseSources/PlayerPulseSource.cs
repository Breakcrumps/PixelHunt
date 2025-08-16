using System;
using Godot;
using PixelHunt.Animation;
using PixelHunt.Characters.Player;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Pulse.PulseSources;

internal enum PulseTechnique { Agility, Strength }

[GlobalClass]
internal sealed partial class PlayerPulseSource : PulseSource
{
  [Export] private PlayerChar? _playerChar;
  [Export] private Animator? _animator;
  [Export] private MoveStateMachine? _moveStateMachine;

  [ExportGroup("Parameters")]
  [Export] private PulseTechnique _pulseTechnique = PulseTechnique.Agility;
  [Export] private int _cooldownFrames;
  
  private GameTime _cooldown = GameTime.Zero;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (_cooldown != GameTime.Zero)
      return;

    if (!@event.IsActionPressed("Pulse"))
      return;

    if (_animator is null)
      return;

    _moveStateMachine?.Transition("StopMoveStrategy");

    _animator.PlayAnimation($"Pulse{Enum.GetName(_pulseTechnique)}");
    _animator.CanProcessRequests = false;

    _cooldown = GameTime.Frame * _cooldownFrames;
  }

  internal void EmitPulse()
  {
    if (_playerChar is null)
      return;

    EmitPulse(new PulseParams { Actor = _playerChar, PulseTechnique = _pulseTechnique });
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_cooldown == GameTime.Zero)
      return;

    _cooldown.Frames -= 1;
  }
}
