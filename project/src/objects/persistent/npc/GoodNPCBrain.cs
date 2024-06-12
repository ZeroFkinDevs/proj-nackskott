using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Мозг доброго NPC
    /// </summary>
    public partial class GoodNPCBrain : NPCBrain
    {
        protected override void Go(){
            var target = ViewComponent.Target;
            if(target!=null){
                if(target.GlobalPosition.DistanceTo(GlobalPosition)>2.0f){
                    controller.GoToPoint(target.GlobalPosition);
                }else{
                    controller.StopMoving();
                    controller.RotateToPoint(target.GlobalPosition);
                }
            }
        }
    }
}