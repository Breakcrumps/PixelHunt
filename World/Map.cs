using Godot;

internal sealed partial class Map : Node3D
{
  public override void _Ready()
  {
    foreach (Node node in FindChildren("*", "StaticBody3D", true))
    {
      if (node is not StaticBody3D staticBody)
        continue;

      staticBody.CollisionLayer = 1 << 4;
    }
  }
}
