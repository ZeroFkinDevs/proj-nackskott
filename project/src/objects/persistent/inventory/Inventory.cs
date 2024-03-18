using Godot;
using System;

namespace Game
{
	public partial class Inventory
	{	
		public partial class Cell : Resource{
			public InventoryItem item = null;
			public int amount = 0;
		}
        public Godot.Collections.Array<Cell> cells = new Godot.Collections.Array<Cell>();

		public event Action<InventoryItem> ActiveLimbChanged;
		private InventoryItem _activeLimb = null;
		public InventoryItem ActiveLimb {
			get{return _activeLimb;}
			set{
				_activeLimb = value;
				ActiveLimbChanged?.Invoke(_activeLimb);
			}
		}

		public Cell GetCellWithItem(InventoryItem item){
			for (int i = 0; i < cells.Count; i++)
			{
				Cell cell = cells[i];
				if(cell.item == item){
					return cell;
				}
			}
			return null;
		}
		public Cell GetOrCreateCell(InventoryItem item){
			Cell cell = GetCellWithItem(item);
			if(cell != null) return cell;

			cell = new Cell();
			cell.item = item;
			cells.Add(cell);
			return cell;
		}

		public Cell AddItem(InventoryItem item, int amount){
			if(amount<=0) return null;
			Cell cell = GetOrCreateCell(item);
			cell.amount += amount;
			return cell;
		}
	}
}