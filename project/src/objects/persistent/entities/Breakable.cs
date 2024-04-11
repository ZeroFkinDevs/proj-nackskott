using Godot;
using System;

namespace Game
{
    public partial class Breakable : Node3D, IDamagable
    {
        [Export]
        public float MaxHealth = 1.0f;
        public float Health = 0.0f;

        public override void _Ready()
        {
            base._Ready();
            Health = MaxHealth;
        }

        public void TakeDamage(float damage, Vector3 position, Node from = null)
        {
            Health -= damage;
            if(Health<=0.0f){
                QueueFree();
            }
        }
    }
}