using Godot;
using Godot.Collections;

namespace Game {
	public partial class ParticlesSystem : Node3D
	{
        public override void _Ready()
        {
            Start();
        }

        public void Start(){
            foreach(var child in GetChildren()){
                if(child is GpuParticles3D particles){
                    particles.Emitting = true;
                }
            }
        }
    }
}