using PixelHunt.Characters;
using PixelHunt.Types;

namespace PixelHunt.Mechanics.Stasis;

internal sealed record StasisParams(Character Actor, GameTime Duration);
