using Godot;
using System;

namespace Game
{
	public interface IPointerControlling
	{
        public void RecieveControl(Vector2 motion);
	}
}