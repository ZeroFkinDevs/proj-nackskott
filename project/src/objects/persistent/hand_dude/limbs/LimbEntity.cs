using Godot;
using System;

namespace Game
{
    
    public partial class LimbEntity : RigidBody3D
    {
        [Export]
		public Godot.Collections.Array<PackedScene> Actions;
        [Export]
        public HookesConnector Connector;
        [Export]
        public Area3D Area;
        public LimbController limbController;
        public bool IsAttacking = false;

        public override void _Ready(){
            Area.BodyEntered += OnBodyEntered;
        }

        public void OnBodyEntered(Node3D body){
            if(!IsAttacking) return;
            if(body is IDamagable damagable){
                damagable.TakeDamage(1.0f, body.GlobalPosition-LinearVelocity, this);
            }
        }

        public void Act(){
            if(Actions.Count==0) return;
            if(IsAttacking) return;
            var action = Actions.PickRandom();
            var instance = action.Instantiate<LimbAction>();
            limbController.Player.GetParent().AddChild(instance);
            instance.GlobalTransform = limbController.Player.rigidBody.GlobalTransform.LookingAt(
                limbController.Player.pointer.GlobalPosition, Vector3.Up);
            
            var target = instance.Act();
            IsAttacking = true;
            Connector.Target = target;

            instance.OnFinished += () => {
                Connector.Target = null;
                IsAttacking = false;
            };
        }
    }
}
