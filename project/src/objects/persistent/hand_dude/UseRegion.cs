using Godot;
using System;

namespace Game
{
	public partial class UseRegion : Area3D
	{
		[Export]
		public Node3D Player;

		[Export]
		public Node3D Indicator;

		public IUsable CurrentUsable;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			BodyEntered += ProcessEnteredObj;
			AreaEntered += ProcessEnteredObj;
			BodyExited += ProcessExitedObj;
			AreaExited += ProcessExitedObj;
		}

        public override void _Process(double delta)
        {
            if(CurrentUsable!=null){
				if(CurrentUsable is Node3D node){
					Indicator.GlobalPosition = node.GlobalPosition;
				}
				if(CurrentUsable.IsEnabled()){
					Indicator.Visible = true;
				}else{
					Indicator.Visible = false;
				}
			}else{
				Indicator.Visible = false;
			}
        }

        void ProcessEnteredObj(Node3D body){
			if(body is IUsable usable){
				CurrentUsable = usable;
			}
		}

		void ProcessExitedObj(Node3D body){
			if(body is IUsable usable){
				if(usable == CurrentUsable){
					CurrentUsable = null;
				}
			}
		}

		public void TryGrip(){
			foreach(Area3D obj in GetOverlappingAreas()){
				if(obj is IGripable gripable){
					if(Player is HandDude hand){
						var p1 = hand.rigidBody.GlobalPosition;
						var p2 = obj.GlobalPosition;
						var space = GetWorld3D().DirectSpaceState;
						var prms = PhysicsRayQueryParameters3D.Create(p1, p2, 2);
						var result = space.IntersectRay(prms);
						// проверить есть ли препятствия
						if(result.Count != 0)
							return;

						if (gripable.IsEnabled()){
							gripable.Grip(Player);
							return;
						}
					}
				}
			}
		}
	}
}