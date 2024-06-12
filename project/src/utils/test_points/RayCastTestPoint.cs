using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class RayCastTestPoint : DebugVisibility
    {
        [Export]
        public RayCast3D rayCast;

        public override void _Process(double delta)
        {
            base._Process(delta);
            var pos = rayCast.GetCollisionPoint();
            GlobalPosition = pos;
        }
    }
}