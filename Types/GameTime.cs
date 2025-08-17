using System;
using System.Numerics;
using Godot;
using ScottPlot.AxisPanels;

namespace PixelHunt.Types;

internal struct GameTime
  : IEquatable<GameTime>, IEqualityOperators<GameTime, GameTime, bool>,
    IComparable<GameTime>, IMultiplyOperators<GameTime, int, GameTime>,
    IAdditionOperators<GameTime, GameTime, GameTime>, IUnaryNegationOperators<GameTime, GameTime>
{
  internal int Frames { get; set; }
  internal readonly float Seconds() => Frames / Engine.PhysicsTicksPerSecond;

  internal GameTime(int frames) { Frames = frames; }


  public readonly bool Equals(GameTime other)
    => Frames == other.Frames;

  public override readonly bool Equals(object? obj)
    => obj is GameTime duration && Equals(duration);


  public override readonly int GetHashCode() => Frames;


  public readonly int CompareTo(GameTime other)
    => Frames.CompareTo(other.Frames);


  public static bool operator ==(GameTime left, GameTime right)
    => left.Equals(right);

  public static bool operator !=(GameTime left, GameTime right)
    => !left.Equals(right);

  public static GameTime operator *(GameTime left, int right)
    => new(left.Frames * right);

  public static GameTime operator +(GameTime left, GameTime right)
    => new(left.Frames + right.Frames);

  public static GameTime operator -(GameTime value)
    => new(-value.Frames);

  internal static GameTime Zero => new(0);
  internal static GameTime Frame => new(1);
  internal static GameTime Second => new(Engine.PhysicsTicksPerSecond);
}
