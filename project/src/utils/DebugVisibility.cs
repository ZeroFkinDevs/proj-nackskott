using Godot;
using System;

namespace Game
{
	public partial class DebugVisibility : Node3D
	{
        public override void _Ready()
        {
            Global.Instance.OnDebugVisibilityChange += OnDebugVisibilityChange;
            OnDebugVisibilityChange(Global.Instance.DebugVisibility);
        }
        public override void _ExitTree()
        {
            base._ExitTree();
        }

        public void OnDebugVisibilityChange(bool visible){
            Visible = visible;
        }
    }
}