using Godot;

[GlobalClass]
public partial class PushbackMoveStrategy : State
{
  [Export] private Player? _character;
  [Export] private Animator? _animator;
  [Export] private AnimationHelper? _animHelper;

  public Vector3 PushbackDirection { private get; set; }

  public override void Enter()
  {
    _animator?.PlayAnimation("Pushback");

    _character!.Velocity = PushbackDirection * _animHelper!.Speed;
  }

  public override void PhysicsProcess(double delta)
  {
    if (Flags.Debug)
      GD.Print("PhysicsProcess called on PushbackMoveStrategy");

    _character!.Velocity = PushbackDirection * _animHelper!.Speed;
  }
}
