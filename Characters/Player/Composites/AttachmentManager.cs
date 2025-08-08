using Godot;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class AttachmentManager : Node
{
  [Export] private Skeleton3D? _skeleton;

  [ExportGroup("Attachments")]
  [Export] private PackedScene? _swordModel;

  public override void _Ready()
  {
    AttachSword(_swordModel);
  }

  internal void AttachSword(PackedScene? swordModel)
  {
    if (_skeleton is null)
      return;

    if (swordModel is null)
      return;

    BoneAttachment3D swordAttachment = new()
    {
      BoneName = "Sword"
    };

    swordAttachment.AddChild(swordModel.Instantiate());

    _skeleton.AddChild(swordAttachment);
  }
}
