using Godot;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Static;
using PixelHunt.Types;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class RubbishRotator : Node
{
  [Export] private RigidBody3D? _rubbish;

  private static readonly FunctionComposer _rotationSpeedComposer = new(
    new LinearComponent { A = .5f },
    new LinearComponent { A = 2f, Start = 120 },
    new LinearComponent { A = 0f, Start = 180 },
    new EndComponent { Start = 360 }
  );

  private GameTime _rotationTime;

  public override void _PhysicsProcess(double delta)
  {
    if (_rubbish is null)
      return;

    if (GlobalInstances.PlayerBuffers is not PlayerBuffers buffers)
      return;

    if (buffers.RotateLongPressed())
    {
      _rotationTime.Frames++;

      _rubbish.AngularVelocity = new Vector3(0f, _rotationSpeedComposer.Execute(_rotationTime), 0f);
    }
    else
    {
      _rotationTime = GameTime.Zero;
    }
  }
}
