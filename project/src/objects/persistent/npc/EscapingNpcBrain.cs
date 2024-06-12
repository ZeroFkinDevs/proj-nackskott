using Godot;
using Godot.Collections;


namespace Game
{
    /// <summary>
    /// Мозг доброго NPC
    /// </summary>
    public partial class EscapingNpcBrain : NPCBrain
    {
        [Export]
        public Array<Node3D> Points;
        private int nextPointIdx = 0;
        private bool finishedMoving = false;
        public Node3D NextPoint{get{return Points[nextPointIdx];}}
        public Node3D CurrentPoint{get{
            if(nextPointIdx==0) return Points[0];
            else return Points[nextPointIdx-1];
        }}

        void GoToNextPoint(){
            if(nextPointIdx>=Points.Count) return;
            controller.GoToPoint(NextPoint.GlobalPosition);
            nextPointIdx += 1;
            finishedMoving = false;
        }

        protected override void Go(){
            var target = ViewComponent.Target;
            if(target!=null){
                var gotToTarget = CurrentPoint.GlobalPosition.DistanceTo(GlobalPosition)<=0.4f;
                if(target.GlobalPosition.DistanceTo(GlobalPosition)<=2.0f){
                    if(nextPointIdx==0){
                        GoToNextPoint();
                    }else if(finishedMoving){
                        GoToNextPoint();
                    }
                    
                } 
                if(gotToTarget && nextPointIdx>0 || finishedMoving){
                    finishedMoving = true;
                    controller.StopMoving();
                    controller.RotateToPoint(target.GlobalPosition);
                }
            }
        }
    }
}