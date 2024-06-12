using Godot;

namespace Game {
	public partial class AudioPlayer : Node3D
	{
		public void PlayAudioAtPosition(AudioStream audio, Vector3 pos){
		    var unit = new AudioUnit();
			unit.Stream = audio;
			unit.Autoplay = true;
			AddChild(unit);
			unit.Play();
		}
		public void PlayAudioAtNode(AudioStream audio, Node3D node){
		    var unit = new AudioUnit();
			unit.Stream = audio;
			unit.Autoplay = true;
			node.AddChild(unit);
			unit.Play();
		}
	}
}
