using Godot;
using System;

namespace Game
{
	public partial class UsablePoint : Node3D, IUsable
	{
		public void Use(Node3D user){
			
		}
		public bool IsEnabled(){
			return true;
		}
	}
}