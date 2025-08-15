using System;
using System.Collections.Generic;
using PixelHunt.Static;
using PixelHunt.Types;
using PixelHunt.Utils;
using ScottPlot;
using ScottPlot.Plottables;

namespace PixelHunt.Algo.FunctionComposition;

internal sealed class FunctionComposer
{
  private readonly List<FunctionComponent> _components;

  internal FunctionComposer(params List<FunctionComponent> components)
  {
    components.InsertionSort();

    components.AssignStartValues();

    components.EnsureEndpoint();

    _components = components;

    if (DebugFlags.GetDebugFlag(this))
      PlotFunction();
  }

  private void PlotFunction()
  {
    Plot plot = new();

    plot.Axes.SetLimits(.0, ResultDuration.Frames, .0, 50);

    FunctionPlot f = plot.Add.Function(t => Execute((int)t));
    f.MinX = 0;
    f.MaxX = ResultDuration.Frames;

    plot.SavePng("Output/Ass.png", 400, 300);
  }

  internal float Execute(int t)
  {
    for (int i = 0; i < _components.Count - 1; i++)
    {
      if (_components[i].Start <= t && t <= _components[i + 1].Start)
      {
        return _components[i].Compute(t);
      }
    }

    throw new ArgumentException("Wrong time for composite function.", nameof(t));
  }

  /// <summary> Frames. </summary>
  internal GameTime ResultDuration => new(_components[^1].Start);
}
