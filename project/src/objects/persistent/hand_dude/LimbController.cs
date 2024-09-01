using Godot;
using System;
using System.Collections.Generic;
using Game.Utils;
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
			limbContainer.GlobalTransform = pose;
		}

		public void OnActiveLimbChanged(LimbInventoryItem limbItem){
			GD.Print(limbItem.Scene);
			PackedScene scene = limbItem.Scene;
			var limbInstance = scene.Instantiate<LimbEntity>();
			Player.CurrentLimbEntity = limbInstance;
			limbInstance.limbController = this;

			Player.AddChild(limbInstance);
			var trans = limbInstance.Transform;
			trans.Origin = limbContainer.GlobalPosition;
			// trans.Basis = Basis.FromEuler(new Vector3(
			// 	Mathf.DegToRad(90.0f), Mathf.DegToRad(180.0f), 0.0f), EulerOrder.Xyz);
			limbInstance.GlobalTransform = trans;

			var joint = new PinJoint3D();
			limbContainer.AddChild(joint);
			joint.NodeA = Player.rigidBody.GetPath();
			joint.NodeB = limbInstance.GetPath();
		}
	}
}