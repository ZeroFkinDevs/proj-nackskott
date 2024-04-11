using Godot;
using System;

namespace Game
{
	public partial class PickableItem : Node3D, IUsable
	{
		[Export]
		public InventoryItem Item;
		[Export]
		public int Amount;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{

		}

		public void Use(Node3D user){
			if(user is HandDude player){
				player.inventory.AddItem(Item, Amount);
				if(Item is LimbInventoryItem limbItem){
					player.inventory.ActiveLimb = limbItem;
				}
				QueueFree();
			}
		}
		public bool IsEnabled(){
			return true;
		}
	}
}