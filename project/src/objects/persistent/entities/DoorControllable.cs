using Godot;
using System;

namespace Game
{
    public partial class DoorControllable : RigidBody3D, IPointerControlling
    {
        [Export]
        private bool _locked = false;
        public bool Locked{get{return _locked;} set{_locked = value; UpdateJoint(); }}

        [Export]
        public Joint3D LockedJoint;
        [Export]
        public Joint3D OpenJoint;

        public void UpdateJoint(){
            Joint3D jointdisable = null;
            Joint3D jointenable = null;
            if(_locked){
                jointdisable = OpenJoint;
                jointenable = LockedJoint;
            }else{
                jointdisable = LockedJoint;
                jointenable = OpenJoint;
            }
            if(jointdisable!=null) jointdisable.NodeA = null;
            if(jointenable!=null) jointenable.NodeA = GetPath();
        }

        public override void _Ready()
        {
            Locked = _locked;
        }

        public override void _PhysicsProcess(double delta)
        {
            var vel = AngularVelocity;
            vel.Y /= 1.5f;
            AngularVelocity = vel;
        }
        public void RecieveControl(Vector2 motion)
        {
            var vel = AngularVelocity;
            vel.Y -= motion.Y * 10.0f;
            AngularVelocity = vel;
        }
    }
}