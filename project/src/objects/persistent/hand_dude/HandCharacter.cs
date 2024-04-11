using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Этот CharacterBody3D нужен для управления игроком как обычным персонажем в игре. 
    /// То есть простое удобное передвижение, без особой физики как у rigidBody.
    /// По идее этот объект невидимый.
    /// </summary>
    public partial class HandCharacter : CharacterBody3D, IDamagable
    {
        [Export]
        private float jumpForce = 60.0f;
        private float gravity = 10.0f;
        private float speed = 4.0f;
        private Vector3 _velocity;

        float _initialRadius = 0.0f;

        [Export]
        public HandAnimationController AnimController;
        [Export]
        public HandDude Player;
        [Export]
        public CollisionShape3D collisionShape;
        SphereShape3D sphereShape;

        public override void _Ready()
        {
            sphereShape = ((SphereShape3D)collisionShape.Shape);
            _initialRadius = sphereShape.Radius;
        }

        public void JumpTo(Vector3 point)
        {
            if(AnimController.IsJumping()) return;
            
            point.Y = GlobalPosition.Y;
            var direction = ((point - GlobalPosition) * new Vector3(1, 0, 1)).Normalized();
            _velocity = direction * jumpForce;
            LookAt(
                GlobalPosition + direction,
                Vector3.Up
            );

            AnimController.Jump();
        }

        public void MoveBy(Vector2 motion)
        {
            if(motion.Length() <= 0.0f) return;

            _velocity.X += motion.X * speed;
            _velocity.Z += motion.Y * speed;
            var lookAtVector = GlobalPosition + new Vector3(motion.X, 0, motion.Y);
            LookAt(
                lookAtVector,
                Vector3.Up
            );
        }

        public override void _PhysicsProcess(double delta)
        {
            if(Player.bodyState == HandDude.BodyState.Controlled) UpdateMovement(delta);
            else AlignWithRigidBody();
        }

        public void UpdateMovement(double delta){
            sphereShape.Radius += (_initialRadius - sphereShape.Radius) / 4f;
            var input = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
            MoveBy(input);

            _velocity.Y -= gravity * (float)delta;
            Velocity = _velocity;
            MoveAndSlide();
            _velocity = Velocity;
            _velocity.X /= 10.0f;
            _velocity.Z /= 10.0f;
        }
        public void AlignWithRigidBody()
        {
            GlobalTransform = Player.rigidBody.GlobalTransform;
            sphereShape.Radius += (0.05f - sphereShape.Radius) / 4.0f;
        }

        public void TakeDamage(float damage, Vector3 position, Node from=null){
            Player.TakeDamage(damage, position, from);
        }
    }
}
