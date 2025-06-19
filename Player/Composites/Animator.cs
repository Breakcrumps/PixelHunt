using System;
using Godot;

public enum Anim { Idle, Walk, Jog, Run, Hover, Rise, Fall, Idle1 }

[GlobalClass]
public partial class Animator : Node
{
  [Export] public Mine? Mine { get; private set; }

  [ExportGroup("Parameters")]
  [Export] private float _fastBlendSpeed = 15f;
  [Export] private float _slowBlendSpeed = 7f;

  private float[] _animValues = new float[Enum.GetNames<Anim>().Length];

  public Anim CurrentAnim { private get; set; } = Anim.Idle;

  public void PlayMovementAnimation(string animName)
  {
    Mine?.MovementAnimation?.Play(animName);
  }

  public override void _PhysicsProcess(double delta)
  {
    SetBlendValues(delta);

    UpdateTree();
  }

  private void SetBlendValues(double delta)
  {
    for (int i = 0; i < _animValues.Length; i++)
    {
      float blendSpeed = (
        i == (int)Anim.Fall
        ? _slowBlendSpeed
        : _fastBlendSpeed
      );

      if (i == (int)CurrentAnim)
        _animValues[i] = Mathf.Lerp(_animValues[i], 1f, blendSpeed * (float)delta);
      else
        _animValues[i] = Mathf.Lerp(_animValues[i], 0f, blendSpeed * (float)delta);
    }
  }

  private void UpdateTree()
  {
    for (int i = 0; i < _animValues.Length; i++)
    {
      string? parameterName = Enum.GetName(typeof(Anim), i);
      Mine!.AnimationTree!.Set($"parameters/{parameterName}/blend_amount", _animValues[i]);
    }
  }
}
