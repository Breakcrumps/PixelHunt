using PixelHunt.Types;
using Godot;

namespace PixelHunt.Characters;

[GlobalClass]
internal abstract partial class Character : CharacterBody3D
{
  [ExportGroup("Parameters")]
  [Export] internal int Health { get; set; } = 100;

  internal abstract void ProcessHit(Attack attack, Vector3 attackerPos);

  private protected void HandleRigidCollision()
  {
    for (int i = 0; i < GetSlideCollisionCount(); i++)
    {
      KinematicCollision3D collision = GetSlideCollision(i);
      GodotObject collider = collision.GetCollider();

      if (collider is not RigidBody3D rigidBody)
        continue;

      Vector3 direction = -collision.GetNormal();
      float massFactor = 50f / rigidBody.Mass;
      rigidBody.ApplyForce(direction * massFactor);

      GD.Print("Colliding smartly!");
    }
  }

  internal void Die()
  {
    ProcessMode = ProcessModeEnum.Disabled;
  }
}
