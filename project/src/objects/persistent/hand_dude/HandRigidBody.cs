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
        public NodePath characterPath;
        private HandCharacter character;

        [Export]
        public float linearSpringStiffness = 1000f;
        [Export]
        public float linearSpringDamping = 10f;
        [Export]
        public float maxLinearForce = 100f;
        [Export]
        public float angularSpringStiffness = 1000f;
        [Export]
        public float angularSpringDamping = 10f;
        [Export]
        public float maxAngularForce = 10f;

        public override void _Ready()
        {
            character = GetNode<HandCharacter>(characterPath);
        }

        /// <summary>
        /// Использует формулу HookesLaw чтобы вычислить силу для перемещения в позицию и поворот HandCharacter.
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateVelocity(double delta)
        {
            if (character == null) return;
            Transform3D targetTransform = character.GlobalTransform;
            Transform3D currentTransform = GlobalTransform;
            Basis rotationDifference = targetTransform.Basis * currentTransform.Basis.Inverse();

            Vector3 positionDifference = targetTransform.Origin - currentTransform.Origin;

            if (positionDifference.LengthSquared() > 5.0f)
            {
                GlobalTransform = targetTransform;
            }
            else
            {
                Vector3 force = HookesLaw(positionDifference, LinearVelocity, linearSpringStiffness, linearSpringDamping);
                force = force.LimitLength(maxLinearForce);
                LinearVelocity += force * (float)delta;
            }

            Vector3 torque = HookesLaw(rotationDifference.GetEuler(), AngularVelocity, angularSpringStiffness, angularSpringDamping);
            torque = torque.LimitLength(maxAngularForce);

            AngularVelocity += torque * (float)delta;
        }
        public override void _PhysicsProcess(double delta)
        {
            UpdateVelocity(delta);
        }

        /// <summary>
        /// Закон Гука (Hooke's law)
        /// </summary>
        /// <param name="displacement"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="stiffness"></param>
        /// <param name="damping"></param>
        /// <returns></returns>
        private Vector3 HookesLaw(Vector3 displacement, Vector3 currentVelocity, float stiffness, float damping)
        {
            return (stiffness * displacement) - (damping * currentVelocity);
        }

        public Vector3 GetViewTargetPoint()
        {
            return Position;
        }
    }
}
