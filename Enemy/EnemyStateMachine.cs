using Godot;

[GlobalClass]
internal partial class EnemyStateMachine : StateMachine
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

  internal override void Process(double delta)
  {
    _currentState?.Process(delta);
  }

  internal override void PhysicsProcess(double delta)
  {
    _currentState?.PhysicsProcess(delta);
  }
}
