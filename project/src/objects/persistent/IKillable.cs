using Godot;
using System;

namespace Game
{
	public interface IKillable
	{
        public void Kill(Node from=null);
	}
}