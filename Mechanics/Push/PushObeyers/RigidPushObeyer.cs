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

    if (_rigidBody.GlobalPosition.DistanceTo(pushParams.Actor.GlobalPosition) > 20f)
      return;

    _rigidBody.ApplyCentralImpulse(Mathf.Pow(_rigidBody.Mass, .5f) * pushParams.Direction * _effectStrength);
  }
}
