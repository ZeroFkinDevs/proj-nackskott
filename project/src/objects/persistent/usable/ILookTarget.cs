using Godot;
using System;

namespace Game
{
	public interface ILookTarget
	{
        public Vector3 GetTargetPoint(Node3D user);
	}
}