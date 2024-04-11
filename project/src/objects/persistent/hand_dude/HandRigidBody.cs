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

        public override void _Ready()
        {

        }

        public override void _Process(double delta)
        {
            if(Player.bodyState == HandDude.BodyState.Controlled) Connector.Active = true;
            else Connector.Active = false;
        }

        public Vector3 GetViewTargetPoint()
        {
            return GlobalPosition;
        }
    }
}
