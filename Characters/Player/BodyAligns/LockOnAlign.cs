using Godot;
using PixelHunt.Characters.Player.CameraModes;
using PixelHunt.Characters.Player.Composites;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.BodyAligns;

[GlobalClass]
internal sealed partial class LockOnAlign : State
{
  [Export] private BodyAlignStateMachine? _bodyAligner;
  
  [Export] private PlayerChar? _playerChar;
  [Export] private CameraStateMachine? _cameraStateMachine;
  [Export] private Skeleton3D? _armature;
  [Export] private Node3D? _cameraPivot;

  internal override void PhysicsProcess(double delta)
  {
    if (_playerChar is null)
      return;

    if (_playerChar.IsOnFloor() || _cameraStateMachine?.CurrentState is FreeCameraMode)
    {
      _bodyAligner?.Transition("DefaultAlign");
      return;
    }

    if (_armature is null)
      return;

    if (_cameraPivot is null)
      return;

    _armature.Rotation = _armature.Rotation with { Y = _cameraPivot.Rotation.Y + Mathf.Pi };
  }
}
