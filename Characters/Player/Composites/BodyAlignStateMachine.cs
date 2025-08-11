using Godot;
using PixelHunt.Parents;

namespace PixelHunt.Characters.Player.Composites;

[GlobalClass]
internal sealed partial class BodyAlignStateMachine : StateMachine
{
  public override void _Ready()
  {
    FillStates();

    Transition("DefaultAlign");
  }
}
