using Godot;

[GlobalClass]
public partial class AudioPlayer : AudioStreamPlayer3D
{
  public override void _Ready()
  {
    EventBus.NotifyCreation(this);

    ProcessMode = ProcessModeEnum.Always;
  }

  public void PlaySound(string filename)
  {
    string filepath = $@"Sounds\{filename}.mp3";

    using FileAccess file = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);

    AudioStreamMP3 stream = new()
    {
      Data = file.GetBuffer((long)file.GetLength())
    };

    Stream = stream;

    Play();
  }
}
