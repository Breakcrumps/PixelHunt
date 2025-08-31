using Godot;
using PixelHunt.Characters;

namespace PixelHunt.Mechanics.Push;

internal sealed record PushParams(Vector3 Direction, Character Actor);
