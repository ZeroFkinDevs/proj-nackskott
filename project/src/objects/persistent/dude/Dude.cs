using Godot;
using System;
using System.Runtime.CompilerServices;

namespace Game
{
    /// <summary>
    /// Интерфейс для классов состояний руки
    /// </summary>
    public interface IDudeState
    {
        public void Init(Dude dude);
        public void Process(Dude dude);
    }

    /// <summary>
    /// Обычное нормальное состояние
    /// </summary>
    public class NormalDudeState: IState, IDudeState
    {
        public void Init(Dude dude) {

        }
        public void Process(Dude dude)
        {
            if (dude.useRegion.CurrentUsable != null){
                if(Input.IsActionJustPressed("use")){
                    dude.SetState(new UsingState());
                }
            }
            if(dude.pointer.GlobalPosition.DistanceTo(dude.Character.GlobalPosition) < 2.0f){
                if(Input.IsActionJustPressed("pointer_press")){
                    dude.pointer.TryToStartControlling(dude);
                }
            }
            if(Input.IsActionJustReleased("pointer_press")){
                dude.pointer.TryToStopControlling(dude);
            }
            dude.Character.Movement = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
        }
    }
    public class UsingState: IState, IDudeState
    {
        public Dude _dude;
        public void OnUsingFinished(){
            if (_dude.useRegion.CurrentUsable != null){
                _dude.useRegion.CurrentUsable.Use(_dude);
            }
            _dude.Character.AnimController.FinishedUsing -= OnUsingFinished;
            _dude.SetState(new NormalDudeState());
        }
        public void Init(Dude dude) {
            _dude = dude;
            _dude.Character.AnimController.FinishedUsing += OnUsingFinished;
            _dude.Character.AnimController.Use();
        }
        public void Process(Dude dude)
        {
            
        }
    }

    /// <summary>
    /// Класс Руки <br/>
    /// этот класс не содержит алгоритмов и сложной логики, он просто совмещает и объединаяет в себе работу с другими состовляющими 
    /// </summary>
    public partial class Dude : Node3D, IStateControlling<IDudeState>, IViewable, ITeleportable, IEntityWithInventory
    {
        private IDudeState currentState;
        
        [Export]
        public Pointer pointer;
        [Export]
        public DudeCharacter Character;

        [Export]
        public UseRegion useRegion;

        [Export]
        public TeleportableObjectType _teleportObjectType = TeleportableObjectType.DUDE;
        [Export]
        public string _teleportToID = "default";
        public string TeleportToID { get{return _teleportToID;} set{_teleportToID = value;}}

        [Export]
        public Inventory _inventory = new Inventory();
        public Inventory inventory{get{return _inventory;}}

        public override void _Ready()
        {
            SetState(new NormalDudeState());
        }
        public override void _Process(double delta)
        {
            currentState?.Process(this);
        }

        public Vector3 GetViewTargetPoint()
        {
            return Character.GlobalPosition;
        }

        public bool SetState(IDudeState state)
        {
            currentState = state;
            state.Init(this);
            return true;
        }

        #region ITeleportable
        public void PlaceAt(Vector3 position)
        {
            var dist = Character.GlobalPosition - GlobalPosition;
            GlobalPosition = position - dist;
        }
        public TeleportableObjectType GetObjectType(){
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
        }
    }
}