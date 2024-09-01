using Godot;
using System;

namespace Game
{
    public partial class DoorControllable : RigidBody3D, IPointerControlling
    {
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