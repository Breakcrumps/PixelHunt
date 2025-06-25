using Godot;

[GlobalClass]
public partial class MoveStateMachine : StateMachine
{
  [Export] private Player? _player;
  [Export] private AnimationHelper? _animHelper;

  public override void _Ready()
  {
    FillStates();

    _currentState = _states["FreeMoveStrategy"];
  }

  public override void PhysicsProcess(double delta)
  {
    _currentState?.PhysicsProcess(delta);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    _currentState?.UnhandledInput(@event);
  }

  public void HandlePushback(Vector3 attackerPos)
  {
    PushbackMoveStrategy pushbackMoveStrategy = (PushbackMoveStrategy)_states["PushbackMoveStrategy"];
    
    if (pushbackMoveStrategy is null)
      return;

    Vector3 pushbackDirection = (_player!.GlobalPosition - attackerPos) with { Y = 0f };

    pushbackMoveStrategy.PushbackDirection = pushbackDirection.Normalized();

    pushbackMoveStrategy.Enter();

    _currentState = pushbackMoveStrategy;

    if (_animHelper is not null)
      _animHelper.AnimationFinished += DisablePushback;
  }

  public void DisablePushback(StringName animName)
  {
    if (animName != "Pushback")
      return;

    _currentState = _states["FreeMoveStrategy"];

    _animHelper!.AnimationFinished -= DisablePushback;
  }
}
