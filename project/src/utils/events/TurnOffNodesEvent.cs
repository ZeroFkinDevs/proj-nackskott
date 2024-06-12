using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class TurnOffNodesEvent : EventNode
    {
        [Export]
        public Godot.Collections.Array<Node3D> Nodes;

        override public void Fire()
        {
            base.Fire();
            foreach (var node in Nodes)
            {
                node.Visible = false;
            }
        }
    }
}