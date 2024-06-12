using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Мозг NPC. Он берет все другие необходимые компоненты 
    /// и используя их управляет контроллером и оставльными нужными частями. 
    /// </summary>
    public partial class NPCBrain : Node3D
    {
        protected NPCController controller;
        public Array<EntityAction> Actions = new Array<EntityAction>();
        public Node3D Target;

        [Export]
        public bool Alive = true;

        public void OnLivingStateChange(bool state){
            GD.Print("brain death");
            Alive = state;
        }
        public override void _Ready()
        {
            CollectChildren();
        }
        public override void _EnterTree()
        {
            base._EnterTree();
            controller = GetParent<NPCController>();
            controller.OnLivingStateChange += OnLivingStateChange;
        }
        public override void _ExitTree()
        {
            base._ExitTree();
            controller.OnLivingStateChange -= OnLivingStateChange;
        }
        void CollectChildren(){
            foreach (var child in GetChildren())
            {
                if(child is EntityAction act){
                    Actions.Add(act);
                    act.Setup(controller);
                }
            }
        }

        [Export]
        public NPCViewComponent ViewComponent;

        public override void _Process(double delta)
        {
            if(Alive){
                Go();
                ThinkOfConfidence();
            }else{
                controller.AnimController.FallDown();
            }
        }

        virtual protected void Go(){
            var target = ViewComponent.Target;
            Target = target;
            if(target!=null){
                controller.GoToPoint(target.GlobalPosition);
                if(!controller.AnimController.IsAttacking()){
                    foreach (var action in Actions)
                    {
                        if(action.CanAct()){
                            action.Act();
                            GD.Print(action.Name);
                        }
                    }
                }
            }
        }

        void ThinkOfConfidence(){
            if(controller.Health<=0){
                controller.Inconfidence=3;
            }else if(controller.Health<=controller.MaxHealth*0.2){
                controller.Inconfidence=2;
            }else if(controller.Health<=controller.MaxHealth*0.6){
                controller.Inconfidence=1;
            }else{
                controller.Inconfidence=0;
            }
        }
    }
}