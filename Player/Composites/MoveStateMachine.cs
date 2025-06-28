using Godot;

[GlobalClass]
internal partial class MoveStateMachine : StateMachine
{
  [Export] private Player? _player;
  [Export] private AnimationHelper? _animHelper;

  public override void _Ready()
  {
    FillStates();

    Transition("FreeMoveStrategy");
  }

  internal override void PhysicsProcess(double delta)
  {
    _currentState?.PhysicsProcess(delta);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    _currentState?.UnhandledInput(@event);
  }

  internal void HandlePushback(Vector3 attackerPos)
  {
    PushbackMoveStrategy pushbackMoveStrategy = (PushbackMoveStrategy)_states["PushbackMoveStrategy"];
    
    if (pushbackMoveStrategy is null)
      return;

    Vector3 pushbackDirection = (_player!.GlobalPosition - attackerPos) with { Y = 0f };

    pushbackMoveStrategy.PushbackDirection = pushbackDirection.Normalized();

    pushbackMoveStrategy.Enter();

    _currentState = pushbackMoveStrategy;
  }
}
