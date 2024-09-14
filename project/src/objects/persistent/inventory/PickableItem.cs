using Godot;
using System;

namespace Game
{
	public partial class PickableItem : Area3D, IUsable, ILookTarget
	{
		[Export]
		public InventoryItem Item;
		[Export]
		public int Amount;
		[Export]
		public Node Container = null;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			
		}

		public void Use(Node3D user){
			if(user is IEntityWithInventory ent){
				ent.AddItem(Item, Amount);
				(Container==null ? this : Container).QueueFree();
			}
		}
		public bool IsEnabled(){
			return true;
		}

        public Vector3 GetTargetPoint(Node3D user)
        {
            return GlobalPosition;
        }
    }
}