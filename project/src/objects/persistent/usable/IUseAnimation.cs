using Godot;
using System;

namespace Game
{
	public interface IUseAnimation
	{
        public string GetAnimationCode(Node3D user);
	}
}