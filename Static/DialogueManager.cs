using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GameSrc.Types;
using Godot;

namespace GameSrc.Static;

using ReplicaList =
  Dictionary<
    string,
    List<Replica>
  >;

internal static class DialogueManager
{
  internal static ReplicaList Dialogue { get; private set; } = [];
  internal static Dictionary<string, ReplicaList> Choices { get; private set; } = [];

  internal static Dictionary<string, bool> Flags { get; private set; } = [];

  static DialogueManager()
  {
    ResetFlags();

    LoadFlags();
  }

  internal static void LoadFlags()
  {
    string flagJson = File.ReadAllText(@"Saves\Flags.json");

    Flags = JsonSerializer.Deserialize<Dictionary<string, bool>>(flagJson)!;
  }

  internal static void WriteFlags()
  {
    string flagJson = JsonSerializer.Serialize(Flags);

    File.WriteAllText(@"Saves\Flags.json", flagJson);
  }

  internal static void ResetFlags()
  {
    string defaultFlagJson = File.ReadAllText(@"Saves\Default\Flags.json");

    File.WriteAllText(@"Saves\Flags.json", defaultFlagJson);
  }

  internal static void UpdateDialogueCache(string sceneName)
  {
    string dialogueJson = File.ReadAllText(@$"Dialogue\{sceneName}.json");
    string choicesJson = File.ReadAllText(@$"Dialogue\Choices\{sceneName}.json");

    Dialogue = JsonSerializer.Deserialize<ReplicaList>(dialogueJson) ?? [];
    Choices = JsonSerializer.Deserialize<Dictionary<string, ReplicaList>>(choicesJson) ?? [];
  }

  internal static void DumpDialogueCache()
  {
    GD.Print("---DIALOGUE DUMP---");

    foreach (var (source, replicas) in Dialogue)
    {
      GD.Print($"{source}:");
      replicas.ForEach(x => GD.Print($"\t{x}"));
    }

    GD.Print("\n");
  }

  internal static void DumpChoiceCache()
  {
    GD.Print("---CHOICES DUMP---");

    foreach (var (name, options) in Choices)
    {
      GD.Print($"{name}:");

      foreach (var (option, replicas) in options)
      {
        GD.Print($"\t{option}:");
        replicas.ForEach(x => GD.Print($"\t\t{x}"));
      }
    }

    GD.Print("\n");
  }

  internal static void DumpFlags()
  {
    foreach (var (name, value) in Flags)
    {
      GD.Print($"{name}: {value}");
    }
  }
}