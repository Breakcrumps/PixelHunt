using Godot;
using PixelHunt.Static;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class GroundRaycast : RayCast3D
{
  [Export] private RigidBody3D? _body;
  [Export] private CollisionShape3D? _collision;

  private float _reach; 

  public override void _Ready()
  {
    if (_collision is null)
      return;

    if (_collision.Shape is not BoxShape3D boxShape)
      return;

    Vector3 size = boxShape.Size;

    Position = new Vector3(0f, size.Y / 2f, 0f);

    _reach = size.MaxAxis() / 2f + .1f;

    CollisionMask = 1 << 4;
  }

  internal bool IsOnFloor()
  {
    if (_body is null)
      return false;

    TargetPosition = ToLocal(GlobalPosition + new Vector3(0f, -_reach, 0f));

    ForceRaycastUpdate();

    return IsColliding();
  }
}
