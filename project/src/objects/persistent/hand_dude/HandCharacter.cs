using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Этот CharacterBody3D нужен для управления игроком как обычным персонажем в игре. 
    /// То есть простое удобное передвижение, без особой физики как у rigidBody.
    /// По идее этот объект невидимый.
    /// </summary>
    public partial class HandCharacter : CharacterBody3D
    {
        [Export]
        private float jumpForce = 20.0f;
        private float gravity = 10.0f;
        private float speed = 4.0f;
        private Vector3 _velocity;

        public void JumpTo(Vector3 point)
        {
            point.Y = Position.Y;
            _velocity = (point - Position) * jumpForce;
            var direction = ((point - Position) * new Vector3(1, 0, 1)).Normalized();
            LookAt(
                Position + direction,
                Vector3.Up
            );
        }

        public void MoveBy(Vector2 motion)
        {
            if(motion.Length() <= 0.0f) return;

            _velocity.X += motion.X * speed;
            _velocity.Z += motion.Y * speed;
            var lookAtVector = Position + new Vector3(motion.X, 0, motion.Y);
            LookAt(
                lookAtVector,
                Vector3.Up
            );
        }

        public override void _PhysicsProcess(double delta)
        {
            var input = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
            MoveBy(input);

            _velocity.Y -= gravity * (float)delta;
            Velocity = _velocity;
            MoveAndSlide();
            _velocity = Velocity;
            _velocity.X /= 10.0f;
            _velocity.Z /= 10.0f;
        }
    }
}
