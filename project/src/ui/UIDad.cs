using Godot;
using System;

namespace UI {
	public partial class UIDad : Node
	{
		[Export]
		public Node SceneContainer; 

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{

		}

		public void PlaceGameScene(Node gameScene){
			gameScene.Reparent(SceneContainer);
		}
	}
}
