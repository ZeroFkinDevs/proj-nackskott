using Godot;

namespace Game {
	public partial class Resources
	{
		public PackedScene BloodParticlesNormal {
			get{
				return GD.Load<PackedScene>("res://scenes/persistent/decorations/blood_stains/blood_particle_1_system.tscn");
			}
		}

		public Node3D EmitBlood(Node3D where, Vector3? lookingAt=null){
			var blood = BloodParticlesNormal.Instantiate<Node3D>();
            where.AddChild(blood);
            blood.Position = Vector3.Zero + Vector3.Up;
            if(lookingAt.HasValue){
                blood.LookAt(lookingAt.GetValueOrDefault(), Vector3.Up);
            }
            blood.Reparent(Global.Instance.GameScene);
			return blood;
		}
	}
}
