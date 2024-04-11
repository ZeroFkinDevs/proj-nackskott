using Godot;
using System;
using System.Threading;

namespace Game
{
	public partial class NPCAnimationController : AnimationController
	{
		[Export]
		public Skeleton3D skeleton3D;
		[Export]
		public Node3D movementMesh;
		[Export]
		public Node3D attackMesh;

		private string movementStateName = "MovementState";
		private string stunStateName = "Stun";
		private string attackStateName = "Attack";
		

		public override void _Ready()
        {
			base._Ready();
			AnimTree.GetAnimation("idle").LoopMode = Animation.LoopModeEnum.Linear;
			AnimTree.GetAnimation("walk").LoopMode = Animation.LoopModeEnum.Linear;
			for (int i = 1; i <= 3; i++)
			{
				AnimTree.GetAnimation("walk_inconfidence_"+i).LoopMode = Animation.LoopModeEnum.Linear;
			}
        }
        public override void _Process(double delta)
        {
            base._Process(delta);

			int boneIdx = skeleton3D.FindBone("attack");
			var pose = skeleton3D.GetBoneGlobalPose(boneIdx);
			var pos = new Vector3(pose.Origin.X, pose.Origin.Z, pose.Origin.Y) / 100f;
			pos = GlobalTransform * pos;
			attackMesh.GlobalPosition = pos;
        }

        public Vector3 GetMovementVector(){
			int boneIdx = skeleton3D.FindBone("movement");
			var pose = skeleton3D.GetBoneGlobalPose(boneIdx);
			var movement = new Vector3(pose.Origin.X, pose.Origin.Z, pose.Origin.Y) / 100f;
			movement = GlobalTransform * movement;
			movementMesh.GlobalPosition = movement;
			movement = movement - GlobalPosition;
			return movement;
		}

		#region movement
		public void Walk(int inconfidence=0)
		{
			if(inconfidence==0){
				SetState(movementStateName, "walk");
			}else{
				SetState(movementStateName, "walk_inconfidence_"+inconfidence);
			}
		}
		public void Idle()
		{
			SetState(movementStateName, "idle");
		}
		#endregion

		public void Attack()
		{
			SetState(attackStateName, "attack");
		}
		public bool IsAttacking()
        {
            return GetState(attackStateName) == "attack";
        }

		public void Stun()
		{
			SetState(stunStateName, "stun");
		}
		public bool IsStunned()
		{
			return GetState(stunStateName) == "stun";
		}
    }
}