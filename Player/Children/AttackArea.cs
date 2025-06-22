using Godot;

[GlobalClass]
public partial class AttackArea : Area3D
{
  [Export] private CollisionShape3D? _collision;
  [Export] private Model? _model;

  [ExportGroup("Parameters")]
  [Export] private int _attackPower = 10;
  [Export] private float _attackPushback;

  public override void _Ready()
  {
    BodyEntered += Attack;

    if (_model is null || _model.AnimationHelper is null)
      return;

    _model.AnimationHelper.HitboxOn += () => { _collision!.Disabled = false; };
    _model.AnimationHelper.HitboxOff += () => { _collision!.Disabled = true; };
  }

  private void Attack(Node3D node)
  {
    if (_collision!.Disabled)
      return;

    if (node is not Character character)
      return;

    Attack attack = new(_attackPower, _attackPushback);

    character.ProcessHit(attack, GlobalPosition);

    _collision!.Disabled = true;
  }
}
