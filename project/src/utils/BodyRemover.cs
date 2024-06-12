using Godot;
using System;

namespace Game
{
	public partial class BodyRemover : Area3D
	{
		[Export]
        public Node3D body;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        void OnBodyEntered(Node3D node){
            if(node == body){
                node.QueueFree();
                QueueFree();
            }
        }
    }
}