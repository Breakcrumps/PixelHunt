using Godot;

public partial class Viewport : SubViewportContainer
{
  public override void _EnterTree()
  {
    GetTree().NodeAdded += node =>
    {
      if (node is MeshInstance3D mesh)
        mesh.AddToGroup("ToApplyMaterial");
    };
  }

  public override void _Ready()
  {
    ApplyMaterial();
  }

  public void ApplyMaterial()
  {
    GetTree().SetGroup("ToApplyMaterial", "material_overlay", new ShaderMaterial() { Shader = ResourceLoader.Load<Shader>("res://Resources/Environment/Viewport.gdshader") });
  }
}
