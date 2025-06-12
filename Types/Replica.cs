using System.Collections.Generic;
using System.Text;

public record Replica(
  string Who = "",
  string Line = "",
  List<string>? Conditions = null,
  List<string>? Actions = null,
  string? Choice = null,
  string? Sound = null
)
{
  public override string ToString()
  {
    StringBuilder stringBuilder = new();

    if (Who != "")
      stringBuilder.Append($"Who: {Who}; ");
    if (Line != "")
      stringBuilder.Append($"Line: {Line}; ");
    if (Conditions != null)
      stringBuilder.Append($"Conditions: {string.Join(", ", Conditions)}; ");
    if (Actions != null)
      stringBuilder.Append($"Actions: {string.Join(", ", Actions)}; ");
    if (Choice != null)
      stringBuilder.Append($"Choice: {Choice}; ");
    if (Sound != null)
      stringBuilder.Append($"Sound: {Sound}; ");

    return $"{stringBuilder}";
  }

  public string RawLine => string.Join("", Line.Split('|'));
}