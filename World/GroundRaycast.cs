using Godot;
using PixelHunt.Static;

namespace PixelHunt.World;

/// <summary>
/// For when a RigidBody needs to know if it's grounded.
/// </summary>
[GlobalClass]
internal sealed partial class GroundRaycast : RayCast3D
{
  [Export] private RigidBody3D? _body;
  [Export] private CollisionShape3D? _collision;

  [Export] private float _margin;

  private float _reach;

  public override void _Ready()
  {
    if (_collision is null)
      return;

    if (_collision.Shape is not BoxShape3D boxShape)
      return;

    Position = new Vector3(0f, boxShape.Size.Y / 2f, 0f);

    _reach = boxShape.Size.MaxAxis() / 2f + _margin;

    CollisionMask = 1 << 4;
  }

  internal bool IsOnFloor()
  {
    TargetPosition = ToLocal(GlobalPosition + new Vector3(0f, -_reach, 0f));

    ForceRaycastUpdate();

    return IsColliding();
  }
}
