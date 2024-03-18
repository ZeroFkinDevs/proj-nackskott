using Godot;
using System;

namespace Game
{
	public interface IUsable
	{
        public void Use(Node3D user);
        public bool IsEnabled();
	}
}