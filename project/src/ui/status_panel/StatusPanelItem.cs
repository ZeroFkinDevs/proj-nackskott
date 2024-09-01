using Godot;
using System;

namespace Game
{
	public partial class StatusPanelItem : Control
	{
		[Export]
		public Label NameLabel;

		public Inventory.Cell ItemCell = null;

		public void SetCell(Inventory.Cell cell){
			if(ItemCell!=null){
				ItemCell.OnUpdate -= Update;
			}
			ItemCell = cell;
			ItemCell.OnUpdate += Update;
			Update();
		}

		public void Update(){
			NameLabel.Text = "";
			if(ItemCell==null) return;
			NameLabel.Text = ItemCell.ItemRes.Name;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}