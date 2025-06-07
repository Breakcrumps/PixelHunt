using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class DialogueManager
{
  public static string Scene { get; set; } = "";

  public static Dictionary<string, List<string>> Dialogue { get; set; } = [];

  public static void UpdateDialogueCache(string sceneName)
  {
    string dialogueJson = File.ReadAllText(@$"Dialogue\{sceneName}.json");

    Dialogue = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(dialogueJson);
  }
}