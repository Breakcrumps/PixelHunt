using GameSrc.Types;
using Godot;

namespace GameSrc.Characters;

internal abstract partial class Character : CharacterBody3D
{
  [ExportGroup("Parameters")]
  [Export] internal int Health { get; set; } = 100;

  internal abstract void ProcessHit(Attack attack, Vector3 attackerPos);

  internal void Die()
  {
    ProcessMode = ProcessModeEnum.Disabled;
  }
}
