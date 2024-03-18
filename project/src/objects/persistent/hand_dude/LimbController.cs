using Godot;
using System;

namespace Game
{
	public partial class LimbController : Node
	{
		[Export]
		public HandDude Player;
		[Export]
		public HandAnimationController handAnimationController;
		[Export]
		public Node3D limbContainer;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Player.inventory.ActiveLimbChanged += OnActiveLimbChanged;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			UpdateLimbContainer(delta);
		}

		public void UpdateLimbContainer(double delta)
		{
			var pose = handAnimationController.GetBaseBoneGlobalPose();
			limbContainer.GlobalPosition = pose.Origin;
		}

		public void OnActiveLimbChanged(InventoryItem limb){
			PackedScene scene = null;
			limb.Data.Get("limb_scene", out scene);
		}
	}
}