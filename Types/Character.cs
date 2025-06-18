using Godot;

public partial class Character : CharacterBody3D
{
  public void Die()
  {
    ProcessMode = ProcessModeEnum.Disabled;
  }
}