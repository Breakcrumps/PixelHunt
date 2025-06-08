using System;

public static class EventBus
{
  public static event Action<string> DialogueInit;
  public static event Action<float> ZFarChange;

  public static void InitDialogue(string source) => DialogueInit?.Invoke(source);
  public static void ChangeZFar(float newZFar) => ZFarChange?.Invoke(newZFar);
}
