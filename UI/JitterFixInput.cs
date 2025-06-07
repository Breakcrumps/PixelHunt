using System;
using Godot;

public partial class JitterFixInput : LineEdit
{
  public Action JitterFixChanged;

  public override void _Ready()
  {
    TextSubmitted += ChangeJitterFix;
  }

  private void ChangeJitterFix(string input)
  {
    if (!input.IsValidFloat())
      return;

    double newJitterFix = input.ToFloat();

    Engine.PhysicsJitterFix = newJitterFix;

    JitterFixChanged?.Invoke();

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
  }
}
