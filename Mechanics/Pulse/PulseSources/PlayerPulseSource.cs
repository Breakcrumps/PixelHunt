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
  [Export] private PlayerAnimator? _animator;
  [Export] private MoveStateMachine? _moveStateMachine;
  [Export] private PlayerBuffers? _playerBuffers;

  [ExportGroup("Parameters")]
  [Export] private PulseTechnique _pulseTechnique = PulseTechnique.Agility;
  [Export] private int _cooldownFrames = 200;

  private GameTime _cooldown = GameTime.Zero;

  public override void _PhysicsProcess(double delta)
  {
    if (_cooldown == GameTime.Zero)
      HandlePulseInput();
    else
      _cooldown.Frames--;
  }

  private void HandlePulseInput()
  {
    if (_playerBuffers is null)
      return;

    if (!_playerBuffers.ButtonLongPressed("Pulse"))
      return;

    if (_playerChar is null || !_playerChar.IsOnFloor())
      return;

    if (_animator is null)
      return;

    _moveStateMachine?.Transition("StopMoveStrategy");

    _animator.PlayAnimation($"Pulse{Enum.GetName(_pulseTechnique)}");
    _animator.CanProcessRequests = false;
  }

  internal void EmitPulse()
  {
    if (_playerChar is null)
      return;

    EmitPulse(new PulseParams { Actor = _playerChar, PulseTechnique = _pulseTechnique });
  }

  internal void StartCooldown()
    => _cooldown = GameTime.Frame * _cooldownFrames;
}
