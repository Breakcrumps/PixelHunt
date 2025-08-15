using System.Collections.Generic;
using PixelHunt.Algo.FunctionComposition;
using PixelHunt.Algo.FunctionComposition.FunctionComponents;

namespace PixelHunt.Utils;

internal static class ComponentListUtils
{
  internal static void InsertionSort(this List<FunctionComponent> components)
  {
    for (int i = 1; i < components.Count; i++)
    {
      int j = i - 1;

      while (j >= 0 && components[j].Start > components[i].Start)
      {
        components[j + 1] = components[j];
        j--;
      }

      components[j + 1] = components[i];
    }
  }

  internal static void AssignStartValues(this List<FunctionComponent> components)
  {
    for (int i = 0; i < components.Count - 1; i++)
    {
      components[i + 1].StartValue = components[i].Compute(components[i + 1].Start);
    }
  }

  internal static void EnsureEndpoint(this List<FunctionComponent> components)
  {
    if (components[^1] is EndComponent)
      return;

    components.Add(new EndComponent() { Start = components[^1].Start });
  }
}
