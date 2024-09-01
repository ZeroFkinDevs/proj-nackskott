using Godot;

namespace Game
{
    public partial class LocationBaseController : Node3D
    {
        [Export]
        public AnimationPlayer AnimPlayer;
        [Export]
        public string AnimationToPlayOnStart = "";

        public override void _Ready()
        {
            StartAnimation();
        }
        void StartAnimation(){
            if(AnimationToPlayOnStart=="") return;
            // AnimPlayer.Play(AnimationToPlayOnStart);
        }
    }
}
