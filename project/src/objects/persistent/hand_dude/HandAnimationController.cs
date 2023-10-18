using Godot;
using System;

namespace Game
{
	public partial class HandAnimationController : AnimationController
	{
		private string handStateName = "HandState";
		private string movementStateName = "MovementState";

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