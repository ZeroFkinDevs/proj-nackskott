using Godot;
using System;

namespace Game
{
	public interface IGripable
	{
        public void Grip(Node3D user);
        public bool IsEnabled();
        public bool CanUnGrip();
        public Node3D GetGripPoint();
	}
}