using System.Collections.Generic;

public record Replica(
  string Who = "",
  string Line = "",
  List<string> Conditions = null,
  List<string> Actions = null,
  string Choice = null
);