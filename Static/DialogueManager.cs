using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public static class DialogueManager
{
  public static string Scene { get; set; } = "";

  public static Dictionary<string, List<Replica>> Dialogue { get; set; } = [];
  public static Dictionary<string, Dictionary<string, List<Replica>>> Choices { get; set; } = [];

  public static void UpdateDialogueCache(string sceneName)
  {
    string dialogueJson = File.ReadAllText(@$"Dialogue\{sceneName}.json");
    string choicesJson = File.ReadAllText(@$"Choices\{sceneName}.json");

    Dialogue = JsonSerializer.Deserialize<Dictionary<string, List<Replica>>>(dialogueJson);
    Choices = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<Replica>>>>(choicesJson);
  }

  public static void DumpDialogueCache()
  {
    GD.Print("---DIALOGUE DUMP---\n");

    foreach (var (source, lines) in Dialogue)
    {
      GD.Print($"{source}:");
      lines.ForEach(x => GD.Print($"\t{x}"));
    }

    GD.Print("\n\n\n");
  }

  public static void DumpChoiceCache()
  {
    GD.Print("---CHOICES DUMP---\n");

    foreach (var (name, options) in Choices)
    {
      GD.Print($"{name}:");

      foreach (var (option, lines) in options)
      {
        GD.Print($"\t{option}:");
        lines.ForEach(x => GD.Print($"\t\t{x}"));
      }
    }

    GD.Print("\n\n\n");
  }
}