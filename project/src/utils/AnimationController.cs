using Godot;
using System;

namespace Game
{
	public partial class AnimationController : Node3D
	{
		// [Export]
		// public NodePath AnimationTreePath;
		[Export]
		public Skeleton3D skeleton3D;

		private AnimationTree _animationThree;
		public AnimationTree AnimTree {get{return _animationThree;}}

		public Transform3D GetBoneGlobalPose(int boneId){
			var pose = skeleton3D.GlobalTransform * skeleton3D.GetBoneGlobalPose(boneId);
			return pose;
		}

        public override void _Ready()
        {
            _animationThree = GetNode<AnimationTree>("AnimationTree");
			AnimTree.AnimationFinished += OnAnimationFinished;
        }

        public override void _Process(double delta)
        {
		    base._Process(delta);
        }

		public virtual void OnAnimationFinished(StringName animationName){
		    
		}

        public void SetState(string transitionNodeName, string stateName)
		{
			AnimTree.Set("parameters/"+transitionNodeName+"/transition_request", stateName);
		}
		public string GetState(string transitionNodeName)
		{
			return (string)AnimTree.Get("parameters/"+transitionNodeName+"/current_state");
		}
    }
}