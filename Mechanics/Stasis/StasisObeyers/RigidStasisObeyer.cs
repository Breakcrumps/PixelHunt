using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Algo.FunctionComposition.FunctionComponents.Modifiers;
using PixelHunt.Algo.FunctionComposition.Functions;
using PixelHunt.Mechanics.Pulse.PulseObeyers;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis.StasisObeyers;

[GlobalClass]
internal sealed partial class RigidStasisObeyer : StasisObeyer
{
  [Export] private RigidBody3D? _body;
  [Export] private RigidPulseObeyer? _pulseObeyer;
  [Export] private RubbishType _rubbleType;

  private GameTime _stasisTime;
  private GameTime _duration;

  internal bool InStasis { get; private set; }

  private float _initialXPos;

  public override void _Ready()
  {
    base._Ready();

    if (_body is null)
      return;

    _body.FreezeMode = RigidBody3D.FreezeModeEnum.Kinematic;

    _initialXPos = _body.GlobalPosition.X;
  }

  private protected override void ObeyStasis(StasisParams stasisParams)
  {
    if (_pulseObeyer is null)
      return;

    if (_body is null)
      return;

    if (!_pulseObeyer.Pulsing)
      return;

    _pulseObeyer.Pulsing = false;

    _stasisTime = GameTime.Zero;
    _duration = stasisParams.Duration;

    InStasis = true;

    _body.LinearVelocity = Vector3.Zero;
    _body.AngularVelocity = Vector3.Zero;

    if (_rubbleType == RubbishType.Wall)
      _body.Freeze = true;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!InStasis)
      return;
    
    if (_body is null)
      return;

    if (_pulseObeyer is null)
      return;
    
    _stasisTime.Frames++;

    _body.GlobalPosition = _body.GlobalPosition with 
    { 
      X = _initialXPos + StasisFunctions.StasisWiggle.ExecuteOrZero(_stasisTime) 
    };

    if (_stasisTime == _duration)
    {
      InStasis = false;

      _body.GravityScale = _pulseObeyer.InitialGravityScale;
      _body.Freeze = false;
    }
  }
}
