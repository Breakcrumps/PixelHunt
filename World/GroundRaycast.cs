using Godot;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class GroundRaycast : RayCast3D
{
  [Export] private RigidBody3D? _body;

  public override void _Ready()
  {
    Position = Position with { Y = .01f };
    
    CollisionMask = 1 << 4;
  }

  internal bool IsOnFloor()
  {
    if (_body is null)
      return false;

    TargetPosition = ToLocal(GlobalPosition + new Vector3(0f, -.5f, 0f));

    return IsColliding();
  }
}
