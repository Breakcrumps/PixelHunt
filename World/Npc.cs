using Godot;

public partial class Npc : CharacterBody3D
{
  private AudioPlayer? _audioPlayer;

  [Export] public int Health { get; set; } = 100;

  private float _pushback;
  private bool _inPushback;
  private Vector3 _pushbackDirection;

  private Npc()
  {
    EventBus.Ready += node =>
    {
      if (node is AudioPlayer audioPlayer)
        _audioPlayer = audioPlayer;
    };
  }

  public override void _PhysicsProcess(double delta)
  {
    Velocity *= .9f;

    Velocity = Velocity with { Y = -10f };

    MoveAndSlide();
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
}
