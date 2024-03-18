using Godot;
using System;

namespace Game
{
	public partial class UseRegion : Area3D
	{
		[Export]
		public HandDude Player;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			BodyEntered += OnBodyEntered;
		}

		void OnBodyEntered(Node3D body){
			if(body is IUsable usable){
				// TODO:
				usable.Use(Player);
			}
		}
	}
}