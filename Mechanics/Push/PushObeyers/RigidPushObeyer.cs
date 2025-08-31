using Godot;

namespace PixelHunt.Mechanics.Push.PushObeyers;

[GlobalClass]
internal sealed partial class RigidPushObeyer : PushObeyer
{
  [Export] private RigidBody3D? _rigidBody;

  private protected override void ObeyPush(PushParams pushParams)
  {
    if (StasisObeyer is null || StasisObeyer.InStasis)
      return;

    if (_rigidBody is null)
      return;

    float distance = pushParams.Actor.GlobalPosition.DistanceTo(_rigidBody.GlobalPosition);
    float effectStrength = _effectStrength / distance;

    _rigidBody.ApplyImpulse(pushParams.Direction * effectStrength);
  }
}
