using Godot;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class GroundRaycast : RayCast3D
{
  [Export] private RigidBody3D? _body;
  [Export] private CollisionShape3D? _collision;

  private BoxShape3D? _boxShape;

  public override void _Ready()
  {
    if (_collision is null)
      return;

    if (_collision.Shape is not BoxShape3D boxShape)
      return;

    _boxShape = boxShape;

    Position = new Vector3(0f, _boxShape.Size.Y / 2f, 0f);

    CollisionMask = 1 << 4;
  }

  internal bool IsOnFloor()
  {
    if (_body is null)
      return false;

    if (_boxShape is null)
      return false;

    float reach = _boxShape.Size[GetDownAxisIndex()];

    TargetPosition = ToLocal(GlobalPosition + new Vector3(0f, -reach, 0f));

    ForceRaycastUpdate();

    return IsColliding();
  }

  private int GetDownAxisIndex()
  {
    int result = -1;
    float bestAngle = int.MaxValue;

    for (int i = 0; i < 3; i++)
    {
      float angle = _body!.GlobalBasis[i].AngleTo(Vector3.Down);

      if (angle < bestAngle)
      {
        result = i;
        bestAngle = angle;
      }
    }

    return result;
  }
}
