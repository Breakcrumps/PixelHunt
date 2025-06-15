using System;
using Godot;

public static class EventBus
{
  public static event Action<Node>? Created;

  public static void NotifyCreation(Node node) => Created?.Invoke(node);
}
