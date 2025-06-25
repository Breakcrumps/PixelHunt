using Godot;

[GlobalClass]
public partial class EnemyStateMachine : StateMachine
{
  [Export] private State? _initialState;

  public override void _Ready()
  {
    FillStates();

    if (_initialState is null)
      return;

    _initialState.Enter();
    _currentState = _initialState;
  }

  public override void Process(double delta)
  {
    _currentState?.Process(delta);
  }

  public override void PhysicsProcess(double delta)
  {
    _currentState?.PhysicsProcess(delta);
  }
}
