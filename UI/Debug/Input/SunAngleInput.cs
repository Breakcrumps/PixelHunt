using Godot;

public partial class SunAngleInput : LineEdit
{
  private Sun? _sun;

  public override void _Ready()
  {
    _sun = (Sun)GetTree().GetFirstNodeInGroup("Sun");

    TextSubmitted += ChangeSunAngle;
  }

  private void ChangeSunAngle(string input)
  {
    if (input == "Seen")
    {
      _sun!.Rotation = new(-40f, -150f, -180f);
      return;
    }
    if (input == "Hidden")
    {
      _sun!.Rotation = new(90f, -150f, -180f);
      return;
    }

    string[] items = input.Split(',');

    if (items.Length != 3)
      return;

    Vector3 axes = new();

    for (int i = 0; i < 3; i++)
    {
      string item = items[i];

      if (!item.IsValidFloat())
        return;

      axes[i] = item.ToFloat();
    }

    _sun!.Rotation = axes;
  }
}
