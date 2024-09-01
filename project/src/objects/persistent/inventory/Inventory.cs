using Godot;
using System;

namespace Game
{
	public partial class Inventory : Resource
	{	
		public partial class Cell : Resource{
			[Signal]
			public delegate void OnUpdateEventHandler();

			[Export]
			public InventoryItem ItemRes = null;
			[Export]
			public int Amount = 0;

			public bool AddAmout(int amount){
				Amount += amount;
				EmitSignal(SignalName.OnUpdate);
				return true;
			}
		}

		[Export]
        public Godot.Collections.Array<Cell> cells = new Godot.Collections.Array<Cell>();

		[Signal]
		public delegate void OnInventoryCellsUpdateEventHandler();

		public delegate void ActiveLimbChangedDelegate(LimbInventoryItem limbItem);
		public event ActiveLimbChangedDelegate ActiveLimbChanged;

		public int ActiveCellIdx = 0;

		private LimbInventoryItem _activeLimb = null;
		public LimbInventoryItem ActiveLimb {
			get{return _activeLimb;}
			set{
				_activeLimb = value;
				ActiveLimbChanged?.Invoke(_activeLimb);
			}
		}

		public void SetCurrentCellByIdx(int idx){
			ActiveCellIdx = idx;
		}
		public void SwitchCurrentCell(int delta){
			int newIdx = ActiveCellIdx + delta;
			if(newIdx>=cells.Count){
				newIdx = 0;
			}else if(newIdx<0){
				newIdx = cells.Count-1;
			}
			SetCurrentCellByIdx(newIdx);
		}

		public Cell GetCellWithItem(InventoryItem item){
			for (int i = 0; i < cells.Count; i++)
			{
				Cell cell = cells[i];
				if(cell.ItemRes == item){
					return cell;
				}
			}
			return null;
		}
		public Cell GetOrCreateCell(InventoryItem item){
			Cell cell = GetCellWithItem(item);
			if(cell != null) return cell;

			cell = new Cell();
			cell.ItemRes = item;
			cells.Add(cell);
			EmitSignal(SignalName.OnInventoryCellsUpdate);
			return cell;
		}

		public Cell AddItem(InventoryItem item, int amount){
			if(amount<=0) return null;
			Cell cell = GetOrCreateCell(item);
			cell.Amount += amount;
			return cell;
		}
	}
}