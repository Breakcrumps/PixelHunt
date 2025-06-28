using Godot;

[GlobalClass]
internal partial class PushbackMoveStrategy : State
{
  [Export] private Player? _character;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  internal Vector3 PushbackDirection { private get; set; }

  internal override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    _character!.Velocity = PushbackDirection * _animHelper!.Speed;
  }

  internal override void PhysicsProcess(double delta)
  {
    if (Flags.Debug)
      GD.Print("PhysicsProcess called on PushbackMoveStrategy");

    _character!.Velocity = PushbackDirection * _animHelper!.Speed;
  }
}
