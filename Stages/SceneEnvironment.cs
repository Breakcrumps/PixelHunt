using Godot;

[GlobalClass]
internal sealed partial class SceneEnvironment : WorldEnvironment
{
  private void ChangeEnvironment(string envName)
  {
    string filepath = $@"Resources\Environment\{envName}.tres";

    Environment = ResourceLoader.Load<Environment>(filepath);
  }
}
