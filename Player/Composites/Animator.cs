using Godot;

public enum Anim { Idle, Walk, Run, SlowWalk }

[GlobalClass]
public partial class Animator : Node
{
  [ExportGroup("Parameters")]
  private float _blendSpeed = 15f;

  public Anim CurrentAnim { private get; set; } = Anim.Idle;

  private float _runValue = 0f;

  public override void _PhysicsProcess(double delta)
  {
    SetBlendValues(delta);

    UpdateTree();
  }

  private void SetBlendValues(double delta)
  {
    switch (CurrentAnim)
    {
      case Anim.Idle:
        _runValue = Mathf.Lerp(_runValue, 0f, _blendSpeed * (float)delta);
        break;
      case Anim.Walk:
        _runValue = Mathf.Lerp(_runValue, .7f, _blendSpeed * (float)delta);
        break;
      case Anim.Run:
        _runValue = Mathf.Lerp(_runValue, 1f, _blendSpeed * (float)delta);
        break;
      case Anim.SlowWalk:
        _runValue = Mathf.Lerp(_runValue, .3f, _blendSpeed * (float)delta);
        break;
    }
  }

  private void UpdateTree()
  {
    EventBus.ChangeBlendParameter("parameters/Run/blend_amount", _runValue);
  }
}
