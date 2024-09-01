using Godot;
using System;

namespace Game
{
	public partial class DudeAnimationController : AnimationController
	{
		private string usingStateName = "usingState";
		private string movementBlendName = "walkBlend";
		private float MovementSpeed = 0.0f;

		public event Action FinishedUsing;

		#region movement
		public void Walk(float _movementSpeed, double delta)
		{
			MovementSpeed = Mathf.Lerp(MovementSpeed, _movementSpeed, (float)delta * 8.0f);
			AnimTree.Set("parameters/"+movementBlendName+"/blend_amount", MovementSpeed / 2.0f);
		}
		public void Use(){
			SetState(usingStateName, "using");
		}
		public override void OnAnimationFinished(StringName animationName){
		    if(animationName == "pick_up_forward"){
				FinishedUsing?.Invoke();
			}
		}
		#endregion
    }
}