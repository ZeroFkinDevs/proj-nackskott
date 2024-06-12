using Godot;

namespace Game {
	public partial class AudioUnit : AudioStreamPlayer3D
	{
        public override void _Process(double delta)
        {
            base._Process(delta);
            if(!Playing) QueueFree();
        }
    }
}
