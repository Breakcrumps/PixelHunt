using Godot;

[GlobalClass]
internal sealed partial class AudioPlayer : AudioStreamPlayer3D
{
  public override void _Ready()
  {
    ProcessMode = ProcessModeEnum.Always;
  }

  internal void PlaySound(string filename)
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
