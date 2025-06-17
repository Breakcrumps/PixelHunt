using Godot;

public enum Anim { Idle, Walk, Jog, Run, Hover, Rise, Fall }

[GlobalClass]
public partial class Animator : Node
{
  private CharacterTree? _characterTree;

  [ExportGroup("Parameters")]
  [Export] private float _fastBlendSpeed = 15f;
  [Export] private float _slowBlendSpeed = 7f;

  public Anim CurrentAnim { private get; set; } = Anim.Idle;

  public float MovingSpeed { private get; set; }

  private float _walkValue;
  private float _jogValue;
  private float _runValue;
  private float _hoverValue;
  private float _riseValue;
  private float _fallValue;

  private Animator()
  {
    EventBus.Ready += node =>
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
    _characterTree?.Set("parameters/Jog/blend_amount", _jogValue);
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
        _jogValue = Mathf.Lerp(_jogValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Walk:
        _characterTree?.Set("parameters/Speed/scale", MovingSpeed / 4f);

        _walkValue = Mathf.Lerp(_walkValue, 1f, _fastBlendSpeed * (float)delta);
        _jogValue = Mathf.Lerp(_jogValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Jog:
        _characterTree?.Set("parameters/Speed/scale", MovingSpeed / 7f);

        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _jogValue = Mathf.Lerp(_jogValue, 1f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Hover:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _jogValue = Mathf.Lerp(_jogValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 1f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Rise:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _jogValue = Mathf.Lerp(_jogValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 1f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 0f, _slowBlendSpeed * (float)delta);
        break;
      case Anim.Fall:
        _walkValue = Mathf.Lerp(_walkValue, 0f, _fastBlendSpeed * (float)delta);
        _jogValue = Mathf.Lerp(_jogValue, 0f, _fastBlendSpeed * (float)delta);
        _hoverValue = Mathf.Lerp(_hoverValue, 0f, _slowBlendSpeed * (float)delta);
        _riseValue = Mathf.Lerp(_riseValue, 0f, _fastBlendSpeed * (float)delta);
        _fallValue = Mathf.Lerp(_fallValue, 1f, _slowBlendSpeed * (float)delta);
        break;
    }
  }
}
