using Godot;
using System;

namespace Game
{
    public partial class Component<T> : Node3D where T : Node3D 
    {
        protected T parent;

        public override void _Ready()
        {
            parent = GetParent<T>();
        }
    }
}
