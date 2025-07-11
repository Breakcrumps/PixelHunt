using System.Collections.Generic;
using System.Text;

namespace GameSrc.Types;

internal sealed record Replica(
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
    if (Conditions is not null)
      stringBuilder.Append($"Conditions: {string.Join(", ", Conditions)}; ");
    if (Actions is not null)
      stringBuilder.Append($"Actions: {string.Join(", ", Actions)}; ");
    if (Choice is not null)
      stringBuilder.Append($"Choice: {Choice}; ");
    if (Sound is not null)
      stringBuilder.Append($"Sound: {Sound}; ");

    return $"{stringBuilder}";
  }

  internal string RawLine => string.Join("", Line.Split('|'));
}