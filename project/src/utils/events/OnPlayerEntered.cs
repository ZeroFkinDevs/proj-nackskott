using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class OnPlayerEntered : Area3D
    {
        public void OnBodyEntered(Node3D body){
            if(body is DudeCharacter){
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
            BodyEntered += OnBodyEntered;
        }
        public override void _ExitTree()
        {
            base._ExitTree();
            BodyEntered -= OnBodyEntered;
        }
    }
}