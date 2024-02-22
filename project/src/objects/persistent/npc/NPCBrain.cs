using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Мозг NPC. Он берет все другие необходимые компоненты 
    /// и используя их управляет контроллером и оставльными нужными частями. 
    /// </summary>
    public partial class NPCBrain : Component<NPCController>
    {
        [Export]
        public NPCViewComponent ViewComponent;

        public override void _Process(double delta)
        {
            Go();
        }

        void Go(){
            var target = ViewComponent.Target;
            if(target!=null){
                parent.GoToPoint(target.GlobalPosition);
            }
        }
    }
}