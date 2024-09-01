using Godot;
using System;

namespace Game
{
	public interface IEntityWithInventory
	{
		public Inventory inventory {get;}
        public void AddItem(InventoryItem item, int amount);
	}
}