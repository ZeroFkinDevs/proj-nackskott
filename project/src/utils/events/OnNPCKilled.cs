using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class OnNPCKilled : Node3D
    {
        [Export]
        protected NPCController Npc;
        public Array<EntityAction> Actions = new Array<EntityAction>();
        public Node3D Target;

        public void OnLivingStateChange(bool state){
            if(!state){
                foreach(var child in GetChildren()){
                    if (child is EventNode ev){
                        ev.Fire();
                    }
                }
            }
        }
        public override void _EnterTree()
        {
            base._EnterTree();
            Npc.OnLivingStateChange += OnLivingStateChange;
        }
        public override void _ExitTree()
        {
            base._ExitTree();
            Npc.OnLivingStateChange -= OnLivingStateChange;
        }
    }
}