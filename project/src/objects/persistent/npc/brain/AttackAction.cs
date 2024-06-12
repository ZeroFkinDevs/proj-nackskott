using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Game
{
    public partial class AttackAction : EntityAction
    {
        [Export]
        public float DistanceToTarget = 2.0f;
        [Export]
        public string AttackName = "attack";

        public override bool CanAct()
        {
            if(base.CanAct()){
                if(brain.Target!=null){
                    if(brain.Target.GlobalPosition.DistanceTo(controller.GlobalPosition) < DistanceToTarget){
                        return true;
                    }
                }
            }
            return false;
        }
        public override void Act()
        {
            base.Act();
            controller.Attack(AttackName);
        }
    }
}