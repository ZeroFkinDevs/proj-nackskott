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
            if (Input.IsActionPressed("pointer_press"))
            {
                handDude.JumpToPointer();
            }
            if (Input.IsActionJustPressed("pointer_action_press"))
            {
                handDude.CurrentLimbEntity?.Act();
                handDude.bodyState = HandDude.BodyState.Free;
                handDude.Character.AnimController.HandGrip();
            }
            if (Input.IsActionJustReleased("pointer_action_press"))
            {
                handDude.bodyState = HandDude.BodyState.Controlled;
                handDude.Character.AnimController.HandFree();
            }
        }
    }
    
    public class StunnedHandState: IState, IHandState
    {
        public void Process(HandDude handDude)
        {
            
        }
    }

    /// <summary>
    /// Класс Руки <br/>
    /// этот класс не содержит алгоритмов и сложной логики, он просто совмещает и объединаяет в себе работу с другими состовляющими 
    /// игрока, такими как StateController, Pointer, HandCharacter, HandRigidBody
    /// </summary>
    public partial class HandDude : Node3D, IStateControlling<IHandState>, IViewable
    {
        private IHandState currentState;
        
        public enum BodyState{
            Controlled,
            Free,
        }
        public BodyState bodyState = BodyState.Controlled;

        [Export]
        public NodePath pointerPath;
        public Pointer pointer;

        [Export]
        public NodePath characterPath;
        private HandCharacter character;
        public HandCharacter Character { get { return character; } }

        [Export]
        public HandRigidBody rigidBody;

        public Inventory inventory = new Inventory();
        public LimbEntity CurrentLimbEntity = null;

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
        }
        
        public override void _Ready()
        {
            InitializeComponents();
            SetState(new FreeHandState());
        }

        public void UpdateBodyState(double delta){
            
        }

        public override void _Process(double delta)
        {
            currentState?.Process(this);
        }

        public void JumpToPointer()
        {
            character.JumpTo(pointer.GlobalPosition);
        }

        private void ApplySettings(GameSettings settings)
        {
            character.Visible = settings.IsInDebug;
        }

        public Vector3 GetViewTargetPoint()
        {
            return rigidBody.GlobalPosition.Lerp(pointer.GlobalPosition, 0.4f);
        }

        public async void Stun(float seconds, Vector3? impulse = null){
            var initialState = currentState;

            SetState(new StunnedHandState());
            bodyState = BodyState.Free;

            if(impulse is Vector3 vec){
                rigidBody.LinearVelocity += vec;
                rigidBody.AngularVelocity += vec;
            }

            await ToSignal(GetTree().CreateTimer(seconds), "timeout");
            SetState(initialState);
            bodyState = BodyState.Controlled;
        }

        public void TakeDamage(float damage, Vector3 position, Node from=null){
            if(currentState is StunnedHandState) return;
            if(from == CurrentLimbEntity) return;
            GD.Print("player damaged");
            Stun(0.5f, position.DirectionTo(rigidBody.GlobalPosition) * new Vector3(1,0,1) * 10.0f);
        }
    }
}
