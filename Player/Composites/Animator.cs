using Godot;

public enum Anim { Idle, Walk, Run, Rise, Fall }

[GlobalClass]
public partial class Animator : Node
{
  [ExportGroup("Parameters")]
  [Export] private float _walkBlendSpeed = 15f;
  [Export] private float _runBlendSpeed = 15f;
  [Export] private float _riseBlendSpeed = 7f;
  [Export] private float _fallBlendSpeed = 7f;

  [Export] private float _shortBlendTime = .25f;
  [Export] private float _longBlendTime = 1.5f;

  public Anim CurrentAnim { private get; set; } = Anim.Idle;

  private float _walkValue = 0f;
  private float _runValue = 0f;
  private float _riseValue = 0f;
  private float _fallValue = 0f;

  public override void _PhysicsProcess(double delta)
  {
    SetBlendValues(delta);

    // UpdateTree();
  }

  // private void UpdateTree()
  // {
  //   EventBus.ChangeBlendParameter("parameters/Walk/blend_amount", _walkValue);
  //   EventBus.ChangeBlendParameter("parameters/Run/blend_amount", _runValue);
  //   EventBus.ChangeBlendParameter("parameters/Rise/blend_amount", _riseValue);
  //   EventBus.ChangeBlendParameter("parameters/Fall/blend_amount", _fallValue);
  // }

  private void SetBlendValues(double delta)
  {
    switch (CurrentAnim)
    {
      case Anim.Idle:
        // _walkValue = Mathf.Lerp(_walkValue, 0f, _walkBlendSpeed * (float)delta);
        // _runValue = Mathf.Lerp(_runValue, 0f, _walkBlendSpeed * (float)delta);
        // _riseValue = Mathf.Lerp(_riseValue, 0f, _riseBlendSpeed * (float)delta);
        // _fallValue = Mathf.Lerp(_fallValue, 0f, _fallBlendSpeed * (float)delta);
        AnimationBus.PlayAnim("Idle", _shortBlendTime);
        break;
      case Anim.Walk:
        // _walkValue = Mathf.Lerp(_walkValue, 1f, _walkBlendSpeed * (float)delta);
        // _runValue = Mathf.Lerp(_runValue, 0f, _walkBlendSpeed * (float)delta);
        // _riseValue = Mathf.Lerp(_riseValue, 0f, _riseBlendSpeed * (float)delta);
        // _fallValue = Mathf.Lerp(_fallValue, 0f, _walkBlendSpeed * (float)delta);
        AnimationBus.PlayAnim("Walk", _shortBlendTime);
        break;
      case Anim.Run:
        // _walkValue = Mathf.Lerp(_walkValue, 0f, _walkBlendSpeed * (float)delta);
        // _runValue = Mathf.Lerp(_runValue, 1f, _walkBlendSpeed * (float)delta);
        // _riseValue = Mathf.Lerp(_riseValue, 0f, _riseBlendSpeed * (float)delta);
        // _fallValue = Mathf.Lerp(_fallValue, 0f, _walkBlendSpeed * (float)delta);
        AnimationBus.PlayAnim("Run", _shortBlendTime);
        break;
      case Anim.Rise:
        // _walkValue = Mathf.Lerp(_walkValue, 0f, _walkBlendSpeed * (float)delta);
        // _runValue = Mathf.Lerp(_runValue, 0f, _walkBlendSpeed * (float)delta);
        // _riseValue = Mathf.Lerp(_riseValue, 1f, _riseBlendSpeed * (float)delta);
        // _fallValue = Mathf.Lerp(_fallValue, 0f, _fallBlendSpeed * (float)delta);
        AnimationBus.PlayAnim("Rise", _shortBlendTime);
        break;
      case Anim.Fall:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _walkBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _walkBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _riseBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 1f, _fallBlendSpeed * (float)delta);
        AnimationBus.PlayAnim("Fall", _longBlendTime);
        break;
    }
  }
}
