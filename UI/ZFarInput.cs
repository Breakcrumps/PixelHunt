using Godot;

public partial class ZFarInput : LineEdit
{
  public override void _Ready()
  {
    TextSubmitted += ChangeZFar;
  }

  private void ChangeZFar(string input)
  {
    if (!input.IsValidInt())
      return;

    int newZFar = input.ToInt();

    EventBus.ChangeZFar(newZFar);

    Clear();

    Input.MouseMode = Input.MouseModeEnum.Captured;
    GetTree().Paused = false;
  }
}
