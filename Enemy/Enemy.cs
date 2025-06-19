using Godot;

public partial class Enemy : Character
{
  private Player? _playerCharacter;
  // private AudioPlayer? _audioPlayer;

  [Export] private Node3D? _body;
  [Export] private CollisionShape3D? _bodyContainer;
  [Export] private Animator? _animator;
  [Export] private StateMachine? _stateMachine;

  [ExportGroup("Parameters")]
  [Export] public int Health { get; set; } = 100;
  [Export] private float _turnSpeed = 10f;

  public override void _Ready()
  {
    _playerCharacter = (Player)GetTree().GetFirstNodeInGroup("Player");
    // _audioPlayer = (AudioPlayer)GetTree().GetFirstNodeInGroup("AudioPlayer");
  }

  public override void _Process(double delta)
  {
    _stateMachine?.Process(delta);
  }

  public override void _PhysicsProcess(double delta)
  {
    _stateMachine?.PhysicsProcess(delta);

    ApplyGravity();

    MoveAndSlide();
  }

  public void AlignBody(double delta, bool inverse = false)
  {
    Vector2 horizontalVelocity = new(Velocity.X, Velocity.Z);

    if (horizontalVelocity == Vector2.Zero)
      return;

    Vector2 refVector = (
      inverse
      ? new Vector2(0, 1)
      : new Vector2(0, -1)
    );

    _bodyContainer!.Rotation = _bodyContainer.Rotation with
    {
      Y = Mathf.LerpAngle(
        _bodyContainer.Rotation.Y,
        horizontalVelocity.AngleTo(refVector),
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

    // _audioPlayer?.PlaySound("amogus");

    Vector3 pushbackDirection = ((GlobalPosition - attackerPos) with { Y = 0f }).Normalized();

    Velocity = pushbackDirection * attack.Pushback;

    _stateMachine?.Transition("PushbackState");

    if (Health == 0)
      QueueFree();
  }
}
