using System;
using Godot;

public static class EventBus
{
  public static event Action<Node>? Ready;

  public static void NotifyReady(Node node) => Ready?.Invoke(node);
}
