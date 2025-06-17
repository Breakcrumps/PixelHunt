using Godot;

public partial class Enemy : CharacterBody3D
{
  private Amogus? _playerCharacter;
  private AudioPlayer? _audioPlayer;

  [Export] private Node3D? _body;
  [Export] private Animator? _animator;

  [ExportGroup("Parameters")]
  [Export] public int Health { get; set; } = 100;
  [Export] private float _speed = 10f;
  [Export] private float _turnSpeed = 10f;

  private float _pushback;
  private bool _inPushback;
  private Vector3 _pushbackDirection;

  private Enemy()
  {
    EventBus.Ready += node =>
    {
      if (node is AudioPlayer audioPlayer)
        _audioPlayer = audioPlayer;
      if (node is Amogus playerCharacter)
        _playerCharacter = playerCharacter;
    };
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_inPushback)
      HandlePushback();
    else
      Move(delta);

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

    Velocity = spatialDirection.Normalized() * _speed;

    AlignBody(delta);
  }

  private void AlignBody(double delta)
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    _body!.Rotation = _body.Rotation with
    {
      Y = Mathf.LerpAngle(
        _body.Rotation.Y,
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

    _animator!.MovingSpeed = horizontalVelocity.Length();

    _animator.CurrentAnim = (
      !IsOnFloor() ? Anim.Fall
      : horizontalVelocity == Vector2.Zero
      ? Anim.Idle : Anim.Jog
    );
  }
}
