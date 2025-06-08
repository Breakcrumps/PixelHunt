using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public static class DialogueManager
{
  public static string Scene { get; set; } = "";

  public static Dictionary<string, List<Replica>> Dialogue { get; set; } = [];
  public static Dictionary<string, Dictionary<string, List<Replica>>> Choices { get; set; } = [];

  public static Dictionary<string, bool> Flags { get; set; } = [];

  static DialogueManager()
  {
    LoadFlags();
  }

  public static void LoadFlags()
  {
    string flagJson = File.ReadAllText(@"Saves\Flags.json");

    Flags = JsonSerializer.Deserialize<Dictionary<string, bool>>(flagJson);
  }

  public static void WriteFlags()
  {
    string flagJson = JsonSerializer.Serialize(Flags);

    File.WriteAllText(@"Saves\Flags.json", flagJson);
  }

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

    foreach (var (source, replicas) in Dialogue)
    {
      GD.Print($"{source}:");
      replicas.ForEach(x => GD.Print($"\t{x}"));
    }

    GD.Print("\n\n\n");
  }

  public static void DumpChoiceCache()
  {
    GD.Print("---CHOICES DUMP---\n");

    foreach (var (name, options) in Choices)
    {
      GD.Print($"{name}:");

      foreach (var (option, replicas) in options)
      {
        GD.Print($"\t{option}:");
        replicas.ForEach(x => GD.Print($"\t\t{x}"));
      }
    }

    GD.Print("\n\n\n");
  }

  public static void DumpFlags()
  {
    foreach (var (name, value) in Flags)
    {
      GD.Print($"{name}: {value}");
    }
  }
}