using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Мозг NPC. Он берет все другие необходимые компоненты 
    /// и используя их управляет контроллером и оставльными нужными частями. 
    /// </summary>
    public partial class NPCBrain : Node3D
    {
        protected NPCController parent;


        public override void _Ready()
        {
            parent = GetParent<NPCController>();
        }

        [Export]
        public NPCViewComponent ViewComponent;

        public override void _Process(double delta)
        {
            Go();
            ThinkOfConfidence();
        }

        void Go(){
            var target = ViewComponent.Target;
            if(target!=null){
                parent.GoToPoint(target.GlobalPosition);
                if(target.GlobalPosition.DistanceTo(parent.GlobalPosition)<2.0f){
                    parent.Attack();
                }
            }
        }

        void ThinkOfConfidence(){
            if(parent.Health<=0){
                parent.Inconfidence=3;
            }else if(parent.Health<=parent.MaxHealth*0.2){
                parent.Inconfidence=2;
            }else if(parent.Health<=parent.MaxHealth*0.6){
                parent.Inconfidence=1;
            }else{
                parent.Inconfidence=0;
            }
        }
    }
}