using Godot;
using System;

namespace Game
{
	public interface IPositionTarget
	{
        public Vector3 GetPositionTarget(Node3D user);
	}
}