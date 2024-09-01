using Godot;
using System;

namespace Game
{
	public partial class StatusPanelInventory : Control
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			CallDeferred("SetupInventory");
		}

		public void SetupInventory(){
			Global.Instance.CurrentInventoryHolder.inventory.OnInventoryCellsUpdate += UpdateCells;
		}

		public void UpdateCells(){
			var cells = Global.Instance.CurrentInventoryHolder.inventory.cells;
			for (int i = 0; i < Math.Min(cells.Count, GetChildCount()); i++)
			{
				var cell = cells[i];
				var UICell = GetChild<StatusPanelItem>(i);
				UICell.SetCell(cell);
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			
		}
	}
}