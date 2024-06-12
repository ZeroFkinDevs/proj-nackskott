using Godot;
using System;

namespace Game
{
    public partial class DoorSystem : Node3D
    {
        [Export]
        public HookesConnector HConnector;
        [Export]
        public RigidBody3D Body;
        [Export]
        public HingeJoint3D Joint;
        [Export]
        public Node3D InitialTarget;

        [Export]
        public float CloseTime = 5.0f;
        float closeTimer = 0.0f;

        bool Deatached = false;

        public override void _Ready()
        {
            base._Ready();
            closeTimer = CloseTime;
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            if(!Deatached) ClosingProcess(delta);
        }

        public void ClosingProcess(double delta){
            var distToInit = HConnector.GlobalPosition.DistanceTo(InitialTarget.GlobalPosition);
            if(distToInit > 0.1f){
                if(closeTimer>0.0f){
                    closeTimer -= (float) delta;
                }else{
                    Close();
                }
            }
        }
        public void Open(){
            HConnector.Active = false;
        }
        public void Close(){
            HConnector.Active = true;
            closeTimer = CloseTime;
        }

        public void Deatach(){
            HConnector.Active = false;
            Deatached = true;
            if(IsInstanceValid(Joint)) Joint.QueueFree();
        }


        public void OnHandleGrip(Node3D handle){
            var wasNotDetached = !Deatached;
            Deatach();
            if(wasNotDetached) Body.ApplyImpulse(-handle.Basis.Z*2.0f, handle.GlobalPosition);
        }
    }
}