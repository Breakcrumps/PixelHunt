using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.Functions;
using PixelHunt.Mechanics.Stasis.StasisObeyers;
using PixelHunt.Types;
using PixelHunt.World;

namespace PixelHunt.Mechanics.Pulse.PulseObeyers;

[GlobalClass]
internal sealed partial class RigidPulseObeyer : PulseObeyer
{
  [Export] private RigidBody3D? _body;
  [Export] private RigidStasisObeyer? _stasisObeyer;
  [Export] private GroundRaycast? _groundRaycast;
  [Export] private RubbishType _rubbishType;

  private FunctionComposer _pulseFunction;

  private GameTime _currentTime;
  private float _initialHeight;

  internal float InitialGravityScale { get; private set; }

  internal bool Pulsing { get; set; }

  private RigidPulseObeyer()
    => _pulseFunction = PulseFunctions.GenerateRubbishFunction(_rubbishType);

  private protected override void ObeyPulse(PulseParams pulseParams)
  {
    if (_groundRaycast is null || !_groundRaycast.IsOnFloor())
      return;

    if (_stasisObeyer is null || _stasisObeyer.InStasis)
      return;

    if (_body is null)
      return;

    InitialGravityScale = _body.GravityScale;
    _body.GravityScale = 0f;

    _currentTime = GameTime.Zero;
    _initialHeight = _body.GlobalPosition.Y;

    Pulsing = true;

    if (_rubbishType == RubbishType.Wall)
      _body.Freeze = true;
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
      _body.GravityScale = InitialGravityScale;

      Pulsing = false;

      _body.Freeze = false;
    }
  }
}
