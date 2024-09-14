using Godot;
using System;

namespace Game
{
	public partial class DudeAnimationController : AnimationController
	{
		private string usingTypeName = "usingType";
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
		public void Use(string animation_code){
			SetState(usingStateName, "using");
			SetState(usingTypeName, animation_code);
		}
		public override void OnAnimationFinished(StringName animationName){
		    if(animationName.ToString().StartsWith("use_")){
				FinishedUsing?.Invoke();
			}
		}
		#endregion
    }
}