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
        public void Process(Dude dude, double delta);
    }

    /// <summary>
    /// Обычное нормальное состояние
    /// </summary>
    public class NormalDudeState: IState, IDudeState
    {
        public void Init(Dude dude) {

        }
        public void Process(Dude dude, double delta)
        {
            if (dude.useRegion.CurrentUsable != null){
                if(dude.useRegion.CurrentUsable.IsEnabled()){
                    if(Input.IsActionJustPressed("use")){
                        dude.SetState(new UsingState());
                    }
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
        public bool used = false;
        public void Use(){
            if (_dude.useRegion.CurrentUsable != null && !used){
                _dude.useRegion.CurrentUsable.Use(_dude);
                used = true;
            }
        }
        public void OnUsingFinished(){
            Use();
            _dude.Character.AnimController.FinishedUsing -= OnUsingFinished;
            AnimWithEvents.OnGlobalEventInvoked -= ProcessGlobalAnimEvent;
            _dude.SetState(new NormalDudeState());
        }
        public void Init(Dude dude) {
            _dude = dude;

            _dude.Character.AnimController.FinishedUsing += OnUsingFinished;
            AnimWithEvents.OnGlobalEventInvoked += ProcessGlobalAnimEvent;

            string animationCode = "pick_up_forward";
            if (_dude.useRegion.CurrentUsable != null){
                if(_dude.useRegion.CurrentUsable is IUseAnimation useAnimation){
                    animationCode = useAnimation.GetAnimationCode(_dude);
                }
            }
            _dude.Character.AnimController.Use(animationCode);
        }
        public void ProcessGlobalAnimEvent(string code){
            if(code=="use"){
                Use();
            }
        }
        public void Process(Dude dude, double delta)
        {
            dude.Character.Movement = Vector2.Zero;
            if (_dude.useRegion.CurrentUsable != null){
                if(_dude.useRegion.CurrentUsable is IPositionTarget positionTarget){
                    var targetPos = positionTarget.GetPositionTarget(dude);
                    var pos = _dude.Character.GlobalPosition;
                    _dude.Character.GlobalPosition = pos.Slerp(new Vector3(targetPos.X, pos.Y, targetPos.Z), (float)delta * 4.0f);
                }
                if(_dude.useRegion.CurrentUsable is ILookTarget lookTarget){
                    var trans = _dude.Character.GlobalTransform;
                    var pos = _dude.Character.GlobalTransform.Origin;
                    var targetPos = lookTarget.GetTargetPoint(dude);
                    trans.Basis = trans.Basis.Slerp(trans.LookingAt(new Vector3(targetPos.X, pos.Y, targetPos.Z)).Basis, (float)delta * 10.0f);
                    _dude.Character.GlobalTransform = trans;
                }
            }
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
            currentState?.Process(this, delta);
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