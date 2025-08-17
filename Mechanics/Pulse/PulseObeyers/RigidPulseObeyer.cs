using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Pulse.PulseObeyers;

[GlobalClass]
internal sealed partial class RigidPulseObeyer : PulseObeyer
{
  [Export] private RigidBody3D? _body;

  private readonly FunctionComposer _pulseFunction = PulseFunctions.GeneratePebbleFunction();

  private GameTime _currentTime;
  private float _initialHeight;

  internal bool Pulsing { get; set; }

  private protected override void ObeyPulse(PulseParams pulseParams)
  {
    if (_body is null)
      return;

    _body.Freeze = true;

    _currentTime = GameTime.Zero;
    _initialHeight = _body.GlobalPosition.Y;

    Pulsing = true;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!Pulsing)
      return;

    if (_body is null)
      return;

    _currentTime.Frames += 1;

    _body.GlobalPosition = _body.GlobalPosition with
    {
      Y = _initialHeight + _pulseFunction.Execute(_currentTime.Frames)
    };

    if (_currentTime == _pulseFunction.ResultDuration)
    {
      _body.Freeze = false;

      Pulsing = false;
    }
  }
}
