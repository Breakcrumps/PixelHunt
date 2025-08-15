using PixelHunt.Characters;
using PixelHunt.Static;
using PixelHunt.Types;
using PixelHunt.Utils;

namespace PixelHunt.Mechanics.Pulse;

internal sealed record PulseParams
{
  internal required Character Actor { get; init; }
  internal int Level { get; init; } = 1;

  internal float Radius => Level * 100f;

  internal GameTime Duration => new(PositionComputer.ResultDuration);

  internal FunctionComposer PositionComputer { get; init; } = PulseFunctions.Default;

  public override string ToString()
    => $"Actor: {Actor.Name}, Level: {Level}, Radius: {Radius}";
}
