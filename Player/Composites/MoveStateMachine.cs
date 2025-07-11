using Godot;
using GameSrc.Parents;
using GameSrc.Player.MoveStrategies;
using GameSrc.Animation;

namespace GameSrc.Player.Composites;

[GlobalClass]
internal sealed partial class MoveStateMachine : StateMachine
{
  [Export] private PlayerChar? _playerChar;
  [Export] private AnimationHelper? _animHelper;

  public override void _Ready()
  {
    FillStates();

    Transition("FreeMoveStrategy");
  }

  internal override void PhysicsProcess(double delta)
  {
    CurrentState?.PhysicsProcess(delta);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    CurrentState?.UnhandledInput(@event);
  }

  internal void HandlePushback(Vector3 attackerPos)
  {
    PushbackMoveStrategy pushbackMoveStrategy = (PushbackMoveStrategy)States["PushbackMoveStrategy"];

    if (pushbackMoveStrategy is null)
      return;

    Vector3 pushbackDirection = (_playerChar!.GlobalPosition - attackerPos) with { Y = 0f };

    pushbackMoveStrategy.PushbackDirection = pushbackDirection.Normalized();

    pushbackMoveStrategy.Enter();

    CurrentState = pushbackMoveStrategy;
  }
}
