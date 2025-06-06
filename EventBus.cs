using System;

public static class EventBus
{
  public static event Action<string> DialogueInit;

  public static void InitDialogue(string message) => DialogueInit?.Invoke(message);
}
