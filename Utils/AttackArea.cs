using PixelHunt.Characters;
using PixelHunt.Types;
using Godot;

namespace PixelHunt.Utils;

[GlobalClass]
internal sealed partial class AttackArea : Area3D
{
  [Export] private CollisionShape3D? _collision;

  [ExportGroup("Parameters")]
  [Export] private int _attackPower = 10;
  [Export] private bool _pushback;

  public override void _Ready()
  {
    BodyEntered += Attack;
  }

  private void Attack(Node3D node)
  {
    if (node is not Character character)
      return;

    Attack attack = new(_attackPower, _pushback);

    character.ProcessHit(attack, GlobalPosition);

    _collision!.Disabled = true;
  }
}
