using Godot;

public partial class SceneEnvironment : WorldEnvironment
{
  public override void _Ready()
  {
    EventBus.EnvironmentChange += ChangeEnvironment;
  }

  private void ChangeEnvironment(string envName)
  {
    string filepath = $@"Resources\Environment\{envName}.tres";

    Environment = ResourceLoader.Load<Environment>(filepath);
  }
}
