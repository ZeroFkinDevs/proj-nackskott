using Godot;
using System;

namespace Game
{
    public partial class EntityAction : Node3D
    {
        [Export]
        public float Timeout = 0.0f;
        public float Timer = 0.0f;

        protected NPCController controller;
        protected NPCBrain brain;

        public override void _Ready()
        {
            brain = GetParent<NPCBrain>();
        }
        public override void _Process(double delta)
        {
            if(Timer > 0.0f){
                Timer -= (float)delta;
            }
        }

        public void Setup(NPCController ctrl){
            controller = ctrl;
        }

        public virtual bool CanAct(){
            return Timer<=0.0f;
        }

        public virtual void Act(){
            Timer = Timeout;
        }
    }
}