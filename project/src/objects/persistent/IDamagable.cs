using Godot;
using System;

namespace Game
{
	public interface IDamagable
	{
        public void TakeDamage(float damage, Vector3 position, Node from=null);
	}
}