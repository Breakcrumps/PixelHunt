using System;
using Godot;

public partial class PhysFpsInput : LineEdit
{
  public event Action PhysFpsChanged;

  public override void _Ready()
  {
    TextSubmitted += ChangePhysFps;
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
    GetTree().Paused = false;
  }
}
