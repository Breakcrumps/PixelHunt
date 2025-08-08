using PixelHunt.Characters;
using Godot;

namespace PixelHunt.World;

[GlobalClass]
internal sealed partial class KillBox : Area3D
{
  public override void _Ready()
  {
    BodyEntered += node =>
    {
      if (node is Character character)
        character.Die();
    };
  }
}
