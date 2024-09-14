using Godot;
using System;

namespace Game
{
	public partial class Bed : Area3D, IUsable, ILookTarget, IUseAnimation, IPositionTarget
	{
		[Export]
		public InventoryItem HaloSleepItem;
		[Export]
		public Teleporter HaloSleepTeleporter;
		[Export]
		public string HaloSleepLocationId;

		[Export]
		public Node3D CharacterPositionPoint;

		private bool enabled = true;

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
				if(ent.inventory.HasAmout(HaloSleepItem, 1)){
					ent.inventory.ConsumeAmount(HaloSleepItem, 1);
					HaloSleepTeleporter.Teleport(HaloSleepLocationId);
				}
			}
		}
		public bool IsEnabled(){
			return enabled;
		}

        public Vector3 GetTargetPoint(Node3D user)
        {
            return GlobalPosition;
        }

        public string GetAnimationCode(Node3D user)
        {
            return "lie_down";
        }

        public Vector3 GetPositionTarget(Node3D user)
        {
            return CharacterPositionPoint.GlobalPosition;
        }
    }
}