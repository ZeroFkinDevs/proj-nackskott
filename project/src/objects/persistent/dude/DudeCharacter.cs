using Godot;
using Godot.Collections;
using System;

namespace Game
{
    /// <summary>
    /// Этот CharacterBody3D нужен для управления игроком как обычным персонажем в игре. 
    /// </summary>
    public partial class DudeCharacter : CharacterBody3D
    {
        private float gravity = 10.0f;
        [Export]
        public float Speed = 4.0f;
        private Vector3 _velocity;

        public Vector2 Movement;

        [Export]
        public DudeAnimationController AnimController;

        public override void _PhysicsProcess(double delta)
        {
            UpdateMovement(delta);
            ProcessRigidPush(delta);
        }
        public void MoveBy(Vector2 motion, double delta)
        {
            if(motion.Length() <= 0.0f){
                return;
            }


            var lookAtVector = GlobalPosition + new Vector3(motion.X, 0, motion.Y);
            var trans = GlobalTransform.LookingAt(lookAtVector, Vector3.Up);
            trans.Basis = GlobalTransform.Basis.Slerp(trans.Basis, (float)delta * 10);
            GlobalTransform = trans;

            _velocity.X += motion.X * Speed;
            _velocity.Z += motion.Y * Speed;

        }
        public void UpdateMovement(double delta){
            var input = Movement;
            MoveBy(input, delta);

            _velocity.Y -= gravity * (float)delta;
            Velocity = _velocity;
            MoveAndSlide();
            _velocity = Velocity;
            _velocity.X /= 1.1f;
            _velocity.Z /= 1.1f;
            AnimController.Walk(new Vector2(_velocity.X, _velocity.Z).Length(), delta);
        }
        private void ProcessRigidPush(double delta){
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var colInfo = GetSlideCollision(i);
                var collider = colInfo.GetCollider();
                if(collider is RigidBody3D body){
                    body.ApplyCentralImpulse(-colInfo.GetNormal());
                }

            }
        }
    }
}