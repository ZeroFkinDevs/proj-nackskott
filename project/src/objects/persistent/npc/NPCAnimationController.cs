using Godot;
using System;
using System.Threading;

namespace Game
{
	public partial class NPCAnimationController : AnimationController
	{
		[Export]
		public Node3D movementMesh;
		[Export]
		public Node3D attackMesh;
		[Export]
		public float MovingSpeed = 1.0f;

		private string movementStateName = "MovementState";
		private string stunStateName = "Stun";
		private string fallState = "FallState";
		private string attackStateName = "Attack";
		
		void SetLoop(string animName){
			var anim = AnimTree.GetAnimation(animName);
			if(anim!=null){
				anim.LoopMode = Animation.LoopModeEnum.Linear;
			}
		}

		public override void _Ready()
        {
			base._Ready();

			SetLoop("idle");
			SetLoop("walk");
			
			for (int i = 1; i <= 3; i++)
			{
				SetLoop("walk_inconfidence_"+i);
			}
        }
        public override void _Process(double delta)
        {
            base._Process(delta);

			int boneIdx = skeleton3D.FindBone("attack");
			if(boneIdx==-1) return;

			var pose = GetBoneGlobalPose(boneIdx);
			// var pos = new Vector3(pose.Origin.X, pose.Origin.Z, pose.Origin.Y);
			if(attackMesh!=null) attackMesh.GlobalPosition = pose.Origin;
        }

        public Vector3 GetMovementVector(){
			int boneIdx = skeleton3D.FindBone("movement");
			if(boneIdx==-1) return Vector3.Zero;

			var pose = GetBoneGlobalPose(boneIdx);
			var movement = pose.Origin;
			if(movementMesh!=null) movementMesh.GlobalPosition = movement;
			movement = movement - GlobalPosition;
			// GD.Print(movement);
			movement *= MovingSpeed;
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
		public void FallDown()
		{
			SetState(fallState, "fall");
		}
		#endregion

		public void Attack(string attackName="attack")
		{
			SetState(attackStateName, attackName);
		}
		public bool IsAttacking()
        {
            return GetState(attackStateName) != "default";
        }
		public override void OnAnimationFinished(StringName animationName){
		    var animName = animationName.ToString();
			if(GetState(attackStateName) == animName){
				SetState(attackStateName, "default");
			}
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