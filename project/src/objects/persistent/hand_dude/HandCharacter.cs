using Godot;
using System;

namespace Game
{
    public partial class HandCharacter : CharacterBody3D
    {
        private float jumpForce = 1.0f;
        private float gravity = 10.0f;
        private float speed = 4.0f;
        private Vector3 _velocity;

        public void JumpTo(Vector3 point)
        {
            _velocity = (point - Position) * jumpForce;
        }

        public override void _PhysicsProcess(double delta)
        {
            _velocity = Velocity;
            var input = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
            _velocity = new Vector3(
                input.X * speed,
                _velocity.Y - gravity * (float)delta,
                input.Y * speed
            );
            Velocity = _velocity;
            MoveAndSlide();
        }
    }
}
