using Godot;
using System;

namespace Game
{
	public partial class HandAnimationController : AnimationController
	{
		[Export]
		public string baseBoneName = "base_bone";
		int baseBoneID = -1;

		private string handStateName = "HandState";
		private string movementStateName = "MovementState";
		private float airTimer = 0.0f;

        public override void _Process(double delta)
        {
			if(airTimer>0.0f) airTimer-=(float)delta;
        }

        public Transform3D GetBaseBoneGlobalPose(){
			baseBoneID = skeleton3D.FindBone(baseBoneName);
			var pose = skeleton3D.GlobalTransform * skeleton3D.GetBoneRest(baseBoneID);
			return pose;
		}

		#region hand state
		public void HandGrip()
		{
			SetState(handStateName, "hand_grip");
		}
		public void HandFree()
		{
			SetState(handStateName, "hand_free");
		}
		#endregion

		#region movement
		public void Jump()
		{
			SetState(movementStateName, "jump");
			airTimer = 0.2f;
		}
		public bool IsJumping()
		{
			return GetState(movementStateName) == "jump";
		}
		public bool IsinAir()
		{
			return airTimer>0.0f;
		}
		public void Idle()
		{
			SetState(movementStateName, "idle");
		}
		#endregion
    }
}