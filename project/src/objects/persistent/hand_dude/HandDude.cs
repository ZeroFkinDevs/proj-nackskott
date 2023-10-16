using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Интерфейс для классов состояний руки
    /// </summary>
    public interface IHandState
    {
        public void Process(HandDude handDude);
    }

    /// <summary>
    /// Обычное нормальное состояние
    /// </summary>
    public class FreeHandState: IState, IHandState
    {
        public void Process(HandDude handDude)
        {
            if (Input.IsActionJustPressed("pointer_press"))
            {
                handDude.JumpToPointer();
            }
        }
    }

    /// <summary>
    /// Класс Руки <br/>
    /// этот класс не содержит алгоритмов и сложной логики, он просто совмещает и объединаяет в себе работу с другими состовляющими 
    /// игрока, такими как StateController, Pointer, HandCharacter, HandRigidBody
    /// </summary>
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
        public HandDude()
        {
            Global.Instance.Settings.OnSettingsChanged += ApplySettings;
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

        private void ApplySettings(GameSettings settings)
        {
            character.Visible = settings.IsInDebug;
        }
    }
}
