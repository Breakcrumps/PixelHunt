using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.Functions;
using PixelHunt.Mechanics.Pulse.PulseObeyers;
using PixelHunt.Static;
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

  private FunctionComposer? _stasisWiggle;
  private int _wiggleAxis;

  private float _initialWigglePos;

  public override void _Ready()
  {
    base._Ready();

    if (_body is null)
      return;

    _body.FreezeMode = RigidBody3D.FreezeModeEnum.Kinematic;

    _initialWigglePos = _body.GlobalPosition.X;
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

    _stasisWiggle = StasisFunctions.GenerateStasisWiggle();
    _wiggleAxis = StasisFunctions.WiggleAxes.RandomItem();

    _initialWigglePos = _body.GlobalPosition[_wiggleAxis];

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

    if (_stasisWiggle is null)
      return;
    
    _stasisTime.Frames++;

    Vector3 newGlobalPosition = _body.GlobalPosition;
    newGlobalPosition[_wiggleAxis] = _initialWigglePos + _stasisWiggle.ExecuteOrZero(_stasisTime);
    _body.GlobalPosition = newGlobalPosition;

    if (_stasisTime == _duration)
    {
      InStasis = false;

      _body.GravityScale = _pulseObeyer.InitialGravityScale;
      _body.Freeze = false;
    }
  }
}
