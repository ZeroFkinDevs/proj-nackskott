using Godot;
using System;

namespace Game
{
    public interface IHandState
    {
        public void Process(HandDude handDude);
    }
    public class FreeHandState: IHandState, IState
    {
        public void Process(HandDude handDude)
        {
            if (Input.IsActionJustPressed("pointer_press"))
            {
                handDude.JumpToPointer();
            }
        }
    }

    public partial class HandDude : Node3D, IStateControlling<IHandState>
    {
        private IHandState currentState;

        [Export]
        public NodePath pointerPath;
        private Pointer pointer;

        [Export]
        public NodePath characterPath;
        private HandCharacter character;
        public HandCharacter Character { get { return character; } }

        [Export]
        public NodePath rigidBodyPath;
        private HandRigidBody rigidBody;

        public bool SetState(IHandState state)
        {
            currentState = state;
            return true;
        }

        private void InitializeComponents()
        {
            pointer = GetNode<Pointer>(pointerPath);
            character = GetNode<HandCharacter>(characterPath);
            rigidBody = GetNode<HandRigidBody>(rigidBodyPath);
        }
        
        public override void _Ready()
        {
            InitializeComponents();
            SetState(new FreeHandState());
        }

        public override void _Process(double delta)
        {
            currentState?.Process(this);
        }

        public void JumpToPointer()
        {
            character.JumpTo(pointer.Position);
        }
    }
}
