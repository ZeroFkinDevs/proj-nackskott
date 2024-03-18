using Godot;
using System;

namespace Game
{
	public partial class HandAnimationController : AnimationController
	{
		[Export]
		public Skeleton3D skeleton3D;
		[Export]
		public string baseBoneName = "base_bone";
		int baseBoneID = -1;

		private string handStateName = "HandState";
		private string movementStateName = "MovementState";

        public override void _Process(double delta)
        {
			baseBoneID = skeleton3D.FindBone(baseBoneName);
        }

        public Transform3D GetBaseBoneGlobalPose(){
			var pose = skeleton3D.GetBoneGlobalPose(baseBoneID);
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
		}
		public bool IsJumping()
		{
			return GetState(movementStateName) == "jump";
		}
		public void Idle()
		{
			SetState(movementStateName, "idle");
		}
		#endregion
    }
}