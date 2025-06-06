using System;
using Godot;

public partial class PhysFpsInput : LineEdit
{
  public Action PhysFpsChanged;

  public override void _Ready()
  {
    TextSubmitted += ChangePhysFps;
  }

  public override void _ExitTree()
  {
    TextSubmitted -= ChangePhysFps;
  }

  private void ChangePhysFps(string input)
  {
    if (!input.IsValidInt())
      return;

    int newPhysFps = input.ToInt();

    Engine.PhysicsTicksPerSecond = newPhysFps;

    PhysFpsChanged?.Invoke();

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
  }
}
