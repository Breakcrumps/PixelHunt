using Godot;

public partial class AttackArea : Area3D
{
  [Export] private CollisionShape3D? _attackCollision;

  [ExportGroup("Parameters")]
  [Export] private int _attackPower = 10;
  [Export] private float _attackPushback;

  public override void _Ready()
  {
    BodyEntered += Attack;
  }

  private void Attack(Node3D node)
  {
    if (_attackCollision!.Disabled)
      return;

    if (node is not Enemy enemy)
      return;

    Attack attack = new(_attackPower, _attackPushback);

    enemy.ProcessHit(attack, GlobalPosition);

    _attackCollision!.Disabled = true;
  }
}
