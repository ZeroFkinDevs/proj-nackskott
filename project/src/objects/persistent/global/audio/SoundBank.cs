using Godot;
using Godot.Collections;

namespace Game {
	public partial class SoundBank : Resource
	{
        [Export]
        public Array<AudioStream> streams;

        AudioStream lastPlayed = null;

        public AudioStream RandomStream{
            get{
                if(streams.Count==0) return null;
                if(streams.Count==1) return streams[0];
                var tmpArr = new Array<AudioStream>(streams);
                if(lastPlayed!=null){
                    tmpArr.Remove(lastPlayed);
                }
                var rand = tmpArr.PickRandom();
                lastPlayed = rand;
                return rand;
            }
        }

        public void PlayAtPosition(Vector3 pos){
            Global.Instance.audioPlayer.PlayAudioAtPosition(RandomStream, pos);
        }
        public void PlayAtNode(Node3D node){
            Global.Instance.audioPlayer.PlayAudioAtNode(RandomStream, node);
        }
    }
}
