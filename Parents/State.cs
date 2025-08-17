using Godot;

namespace PixelHunt.Parents;

[GlobalClass]
internal abstract partial class State : Node
{
  internal virtual void Enter() { }

  internal virtual void Exit() { }

  internal virtual void Process(double delta) { }

  internal virtual void PhysicsProcess(double delta) { }

  internal virtual void UnhandledInput(InputEvent @event) { }
}
