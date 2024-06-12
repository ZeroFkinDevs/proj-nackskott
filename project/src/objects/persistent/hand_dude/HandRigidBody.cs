using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Этот RigidBody3D содержит в себе модель руки и создает эффект ее физичности, то есть он всегда стремиться к HandCharacter.
    /// </summary>
    public partial class HandRigidBody : RigidBody3D, IViewable
    {
        [Export]
        public HandCharacter character;
        [Export]
        public HandDude Player;
        [Export]
        public HookesConnector Connector;
        [Export]
        public HookesConnector GripConnector;
        [Export]
        public AudioStreamPlayer3D Rustling;

        public Node3D GripPoint = null;
        public IGripable GripableTarget = null;
        public bool CanUnGrip {get{
            if(GripableTarget is Node3D node){
                if(!IsInstanceValid(node)) return true;
            }
            if(GripableTarget!=null){
                return GripableTarget.CanUnGrip();
            };
            return true;
        }}

        public override void _Ready()
        {

        }

        public override void _Process(double delta)
        {
            if(Player.bodyState == HandDude.BodyState.Controlled){
                Connector.Active = true;
                GripConnector.Active = false;
            }else{
                Connector.Active = false;
                if(GripPoint!=null){
                    GripConnector.Active = true;
                }else{
                    GripConnector.Active = true;
                }
            }
            UpdateAudio();
        }

        void UpdateAudio(){
            if(Player.bodyState == HandDude.BodyState.Free && !character.AnimController.IsinAir()){
                var volume = LinearVelocity.Length()*35.0f-50.0f;
                Rustling.VolumeDb += (volume - Rustling.VolumeDb) / 6.0f;
            }else{
                Rustling.VolumeDb += (-100 - Rustling.VolumeDb) / 20.0f;
            }
        }

        public void Grip(IGripable gripable){
            var point = gripable.GetGripPoint();
            GripableTarget = gripable;
            GripPoint = point;
            GripConnector.Target = point;
        }
        public void EndGrip(){
            GripableTarget = null;
            GripPoint = null;
            GripConnector.Target = null;
        }

        public Vector3 GetViewTargetPoint()
        {
            return GlobalPosition;
        }

        public void EmitBlood(Vector3? lookingAt=null){
            var blood = Global.Instance.resources.EmitBlood(Player.rigidBody, lookingAt);
        }
    }
}
