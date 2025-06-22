using Godot;

public abstract partial class Character : CharacterBody3D
{
  [ExportGroup("Parameters")]
  [Export] public int Health { get; set; } = 100;

  public abstract void ProcessHit(Attack attack, Vector3 attackerPos);

  public void Die()
  {
    ProcessMode = ProcessModeEnum.Disabled;
  }
}