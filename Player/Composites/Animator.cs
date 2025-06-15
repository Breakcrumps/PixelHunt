using Godot;

public enum Anim { Idle, Walk, Run, Hover, Rise, Fall }

[GlobalClass]
public partial class Animator : Node
{
  private CharacterTree? _characterTree;

  [ExportGroup("Parameters")]
  [Export] private float _fastBlendSpeed = 15f;
  [Export] private float _slowBlendSpeed = 7f;

  public Anim CurrentAnim { private get; set; } = Anim.Idle;

  private float _walkValue = 0f;
  private float _runValue = 0f;
  private float _hoverValue = 0f;
  private float _riseValue = 0f;
  private float _fallValue = 0f;

  private Animator()
  {
    EventBus.Created += node =>
    {
      if (node is CharacterTree characterTree)
        _characterTree = characterTree;
    };
  }

  public override void _PhysicsProcess(double delta)
  {
    SetBlendValues(delta);

    UpdateTree();
  }

  private void UpdateTree()
  {
    _characterTree?.Set("parameters/Walk/blend_amount", _walkValue);
    _characterTree?.Set("parameters/Run/blend_amount", _runValue);
    _characterTree?.Set("parameters/Hover/blend_amount", _hoverValue);
    _characterTree?.Set("parameters/Rise/blend_amount", _riseValue);
    _characterTree?.Set("parameters/Fall/blend_amount", _fallValue);
  }

  private void SetBlendValues(double delta)
  {
    switch (CurrentAnim)
    {
      case Anim.Idle:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Walk:
        _walkValue = Mathf.Lerp(_walkValue, 1f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Run:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 1f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Hover:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 1f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Rise:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 1f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Fall:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _runValue = Mathf.Lerp(_runValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 1f, _slowBlendSpeed * (float)delta);
        break;
    }
  }
}
