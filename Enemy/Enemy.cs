using Godot;

public partial class Enemy : Character
{
  private Amogus? _playerCharacter;
  private AudioPlayer? _audioPlayer;

  [Export] private Node3D? _body;
  [Export] private CollisionShape3D? _bodyContainer;
  [Export] private Animator? _animator;

  [ExportGroup("Parameters")]
  [Export] public int Health { get; set; } = 100;
  [Export] public float Speed { get; private set; } = .3f;
  [Export] private float _turnSpeed = 10f;

  private float _pushback;
  private bool _inPushback;
  private Vector3 _pushbackDirection;

  public override void _Ready()
  {
    _playerCharacter = (Amogus)GetTree().GetFirstNodeInGroup("Player");
    _audioPlayer = (AudioPlayer)GetTree().GetFirstNodeInGroup("AudioPlayer");
  }

  public override void _PhysicsProcess(double delta)
  {
    ApplyGravity();

    MoveAndSlide();

    Animate();
  }

  private void HandlePushback()
  {
    Velocity *= .9f;

    if (Mathf.Abs(Velocity.X) < .01f && Mathf.Abs(Velocity.Z) < .01f)
      _inPushback = false;
  }

  private void Move(double delta)
  {
    Vector3 spatialDirection = _playerCharacter!.GlobalPosition - GlobalPosition;

    Velocity = spatialDirection.Normalized() * Speed;

    AlignBody(delta);
  }

  public void AlignBody(double delta)
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _bodyContainer!.Rotation = _bodyContainer.Rotation with
    {
      Y = Mathf.LerpAngle(
        _bodyContainer.Rotation.Y,
        horizontalVelocity.AngleTo(new Vector2(0, -1)),
        _turnSpeed * (float)delta
      )
    };
  }

  private void ApplyGravity()
  {
    if (IsOnFloor())
      return;

    Velocity = Velocity with { Y = -10f };
  }

  public void ProcessHit(Attack attack, Vector3 attackerPos)
  {
    Health -= attack.Power;

    if (Flags.Debug)
      GD.Print($"{Name} was hit for {attack.Power}HP, {Health}HP left.");

    _audioPlayer?.PlaySound("amogus");

    _pushbackDirection = ((GlobalPosition - attackerPos) with { Y = 0f }).Normalized();
    float _pushback = attack.Pushback;

    Velocity = _pushbackDirection * _pushback;
    MoveAndSlide();

    _inPushback = true;

    if (Health == 0)
      QueueFree();
  }

  private void Animate()
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    _animator!.CurrentAnim = (
      !IsOnFloor() ? Anim.Fall
      : horizontalVelocity == Vector2.Zero
      ? Anim.Idle : Anim.Walk
    );
  }
}
