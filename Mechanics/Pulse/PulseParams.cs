using PixelHunt.Characters;
using PixelHunt.Mechanics.Pulse.PulseSources;

namespace PixelHunt.Mechanics.Pulse;

internal sealed record PulseParams
{
  internal required Character Actor { get; init; }
  internal int Level { get; init; } = 1;

  internal PulseTechnique PulseTechnique { get; init; } = PulseTechnique.Agility;

  internal float Radius => Level * 100f;

  public override string ToString()
    => $"Actor: {Actor.Name}, Level: {Level}, Radius: {Radius}";
}
