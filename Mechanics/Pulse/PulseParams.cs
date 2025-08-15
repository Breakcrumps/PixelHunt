using PixelHunt.Characters;

namespace PixelHunt.Mechanics.Pulse;

internal sealed record PulseParams
{
  internal required Character Actor { get; init; }
  internal int Level { get; init; } = 1;

  internal float Radius => Level * 100f;

  public override string ToString()
    => $"Actor: {Actor.Name}, Level: {Level}, Radius: {Radius}";
}
