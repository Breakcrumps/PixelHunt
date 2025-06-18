using Godot;
using System;

public abstract partial class State : Node
{
  public event Action<string>? Transition;

  public abstract void Enter();

  public abstract void Exit();

  public abstract void Process(double delta);

  public abstract void PhysicsProcess(double delta);
}