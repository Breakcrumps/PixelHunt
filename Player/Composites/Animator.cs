using System;
using Godot;

public enum Anim { Idle, Walk, Jog, Run, Hover, Rise, Fall }

[GlobalClass]
public partial class Animator : Node
{
  [Export] private Mine? _mine;

  [ExportGroup("Parameters")]
  [Export] private float _fastBlendSpeed = 15f;
  [Export] private float _slowBlendSpeed = 7f;

  private float[] _animValues = new float[Enum.GetNames<Anim>().Length];

  public Anim CurrentAnim { private get; set; } = Anim.Idle;
  public float MovingSpeed { private get; set; }

  public override void _PhysicsProcess(double delta)
  {
    HandleAnimationSpeed();

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
      _mine!.AnimationTree!.Set($"parameters/{Enum.GetName(typeof(Anim), i)}/blend_amount", _animValues[i]);
  }

  private void HandleAnimationSpeed()
  {
    if (CurrentAnim == Anim.Walk)
      SetAnimationSpeed(MovingSpeed / 4f);
    else if (CurrentAnim == Anim.Jog)
      SetAnimationSpeed(MovingSpeed / 7f);
    else
      SetAnimationSpeed(1f);
  }

  private void SetAnimationSpeed(float speed)
  {
    _mine!.AnimationTree!.Set("parameters/Speed/scale", speed);
  }
}
