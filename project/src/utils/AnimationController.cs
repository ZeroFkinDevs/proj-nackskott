using Godot;
using System;

namespace Game
{
	public partial class AnimationController : Node3D
	{
		// [Export]
		// public NodePath AnimationTreePath;
		private AnimationTree _animationThree;
		public AnimationTree AnimTree {get{return _animationThree;}}

        public override void _Ready()
        {
            _animationThree = GetNode<AnimationTree>("AnimationTree");
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