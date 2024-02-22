using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Позволяет видеть других существ
    /// </summary>
    public partial class NPCViewComponent : Node3D
    {
        [Export]
        public NodePath AreaPath;
        Area3D Area;

        private const double CHECK_INTERVAL = 0.5;
        private double timer = 0;

        public Node3D Target;

        public override void _Ready()
        {
            base._Ready();
            Area = GetNode<Area3D>(AreaPath);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            CheckViewCycle(delta);
        }

        void CheckViewCycle(double delta){
            timer -= delta;
            if(timer<=0){
                timer = CHECK_INTERVAL;
                var body = GetSeeingTarget();
                
                if (body != null){
                    Target = body;
                }
            }
        }

        /// <summary>
        /// проверить подходит ли тело для цели
        /// </summary>
        /// <param name="body">тело для проверки</param>
        /// <returns></returns>
        bool CheckBody(Node3D body)
        {
            if(body is HandCharacter){
                return true;
            }
            return false;
        }
        
        Node3D GetSeeingTarget()
        {
            var bodies = Area.GetOverlappingBodies();
            foreach (var body in bodies){
                // проверить подходит ли тело для цели
                if(CheckBody(body)){
                    // отбросить луч
                    var p1 = GlobalPosition;
                    var p2 = body.GlobalPosition;
                    var space = GetWorld3D().DirectSpaceState;
                    var prms = PhysicsRayQueryParameters3D.Create(p1, p2, 2);
                    var result = space.IntersectRay(prms);
                    // проверить есть ли препятствия
                    if(result.Count == 0)
                        return body;
                }
            }
            return null;
        }
    }
}
