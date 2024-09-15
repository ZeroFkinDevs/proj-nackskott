using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class SetAnimControllerState : EventNode
    {
        [Export]
        public AnimationController animController;
        [Export]
        public string StateName = "";
        [Export]
        public string Value = "";

        override public void Fire()
        {
            base.Fire();
            animController.SetState(StateName, Value);
        }
    }
}