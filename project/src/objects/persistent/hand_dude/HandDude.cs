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
            if (handDude.useRegion.CurrentUsable != null){
                handDude.useRegion.CurrentUsable.Use(handDude);
            }

            if (Input.IsActionPressed("pointer_press"))
            {
                if(handDude.bodyState == HandDude.BodyState.Controlled){
                    handDude.JumpToPointer();
                }
            }
            if (Input.IsActionJustPressed("pointer_action_press"))
            {
                handDude.CurrentLimbEntity?.Act();
                handDude.bodyState = HandDude.BodyState.Free;
                handDude.Character.AnimController.HandGrip();
                handDude.TryGripSmth();
            }
            if (Input.IsActionJustReleased("pointer_action_press"))
            {
                if(handDude.rigidBody.CanUnGrip){
                    handDude.bodyState = HandDude.BodyState.Controlled;
                    handDude.Character.AnimController.HandFree();
                    handDude.EndGrip();
                }
            }
            if (Input.IsActionJustPressed("scroll_up"))
            {
                handDude.inventory.SwitchCurrentCell(-1);
            }
            if (Input.IsActionJustPressed("scroll_down"))
            {
                handDude.inventory.SwitchCurrentCell(1);
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
    public partial class HandDude : Node3D, IStateControlling<IHandState>, IViewable, IKillable, ITeleportable, IEntityWithInventory
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
        [Export]
        public UseRegion useRegion;
        
        [Export]
        public TeleportableObjectType _teleportObjectType = TeleportableObjectType.HAND_DUDE;
        [Export]
        public string _teleportToID = "default";

        public string TeleportToID{get{return _teleportToID;} set{_teleportToID = value;}}

        [Export]
        public Inventory _inventory = new Inventory();
        public Inventory inventory{get{return _inventory;}}
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

            pointer.workingPlane.Y = Character.GlobalPosition.Y;
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

            if(seconds!=0.0f){
                await ToSignal(GetTree().CreateTimer(seconds), "timeout");
                SetState(initialState);
                bodyState = BodyState.Controlled;
            }
        }

        #region Grip
        public void TryGripSmth(){
            useRegion.TryGrip();
        }
        public void Grip(IGripable gripable){
            rigidBody.Grip(gripable);
        }
        public void EndGrip(){
            rigidBody.EndGrip();
        }
        public void UnGripObject(){
            rigidBody.EndGrip();
            Character.AnimController.HandFree();
            bodyState = BodyState.Controlled;
        }
        #endregion

        public void TakeDamage(float damage, Vector3 position, Node from=null){
            if(currentState is StunnedHandState) return;
            if(from == CurrentLimbEntity) return; //TODO: carefull with multiple limbs
            GD.Print("player damaged");
            Stun(0.5f, position.DirectionTo(rigidBody.GlobalPosition) * new Vector3(1,0,1) * 6.0f);
            if(from is Node3D n3d){
                rigidBody.EmitBlood(n3d.GlobalPosition);
            }else{
                rigidBody.EmitBlood();
            }
        }

        public void Kill(Node from=null){
            Stun(0.0f, new Vector3(0, 0, 0));
            if(from is Node3D n3d){
                rigidBody.EmitBlood(n3d.GlobalPosition);
            }else{
                rigidBody.EmitBlood();
            }
        }

        #region ITeleportable
        public void PlaceAt(Vector3 position){
            // rigidBody.GlobalPosition = position;
            // Character.GlobalPosition = position;
            var dist = Character.GlobalPosition - GlobalPosition;
            GlobalPosition = position - dist;
        }

        public TeleportableObjectType GetObjectType()
        {
            return _teleportObjectType;
        }

        public void Deactivate()
        {
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
        }

        public void Activate()
        {
            Visible = true;
            ProcessMode = ProcessModeEnum.Inherit;
        }
        #endregion

        public void AddItem(InventoryItem item, int amount)
        {
            inventory.AddItem(item, amount);
            if(item is LimbInventoryItem limbItem){
                inventory.ActiveLimb = limbItem;
            }
        }
    }
}
