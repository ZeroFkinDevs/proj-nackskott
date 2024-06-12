using Godot;
using System;

namespace Game
{
    public partial class DamageArea : Area3D
    {
        [Export]
        public float Damage = 1.0f;
        [Export]
        public Node3D FromPosition = null;
        [Export]
        public Node3D FromEntity = null;

        public override void _Ready()
        {
            base._Ready();
            BodyEntered += OnBodyEntered;
        }

        public void OnBodyEntered(Node3D body){
            if(body is IDamagable damagable){
                var fromEnt = GetParent<Node>();
                var fromPos = GlobalPosition;
                if(FromPosition!=null) fromPos = FromPosition.GlobalPosition;
                if(FromEntity!=null) fromEnt = FromEntity;
                
                if(body.GetInstanceId() == fromEnt.GetInstanceId()) return;
                damagable.TakeDamage(Damage, fromPos, fromEnt);
            }
        }
    }
}