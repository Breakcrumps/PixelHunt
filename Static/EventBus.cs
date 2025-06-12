using System;

public static class EventBus
{
  public static event Action<string> DialogueInit;
  public static event Action<float> ZFarChange;
  public static event Action<string> SoundPlay;
  public static event Action<string> EnvironmentChange;

  public static void InitDialogue(string source) => DialogueInit?.Invoke(source);
  public static void ChangeZFar(float newZFar) => ZFarChange?.Invoke(newZFar);
  public static void PlaySound(string filename) => SoundPlay?.Invoke(filename);
  public static void ChangeEnvironment(string envName) => EnvironmentChange?.Invoke(envName);
}
