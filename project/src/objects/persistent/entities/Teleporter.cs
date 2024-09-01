using Godot;
using System;

namespace Game
{
    public partial class Teleporter : Node3D
    {
        [Export]
        public string ID;
        [Export]
        public TeleportableObjectType ObjectType = TeleportableObjectType.HAND_DUDE;

        [Signal]
		public delegate void OnTeleportStartEventHandler();
        [Signal]
		public delegate void OnTeleportEndEventHandler(Node player);

        public override void _Ready()
        {
            base._Ready();
            if(GetParent() is IIdentable identable){
                ID = identable.GetID();
            }
            CallDeferred("DeferredReady");
        }

        public void DeferredReady()
        {   
            IViewFollower<Node3D> viewFollower = null;
            foreach(var node in Global.Instance.PlayerSystem.GetChildren()){
                if(node is IViewFollower<Node3D> follower){
                    viewFollower = follower;
                }
            }
            foreach(var node in Global.Instance.PlayerSystem.GetChildren()){
                if(node is ITeleportable teleportable){
                    if(teleportable.TeleportToID == ID){
                        if(ObjectType == teleportable.GetObjectType()){
                                if(node is IEntityWithInventory withInventory){
                                    Global.Instance.CurrentInventoryHolder = withInventory;
                                }
                            if(viewFollower != null){
                                if(node is Node3D viewable){
                                    viewFollower.SetViewTarget(viewable);
                                }
                            }
                            teleportable.Activate();
                        }else{
                            teleportable.Deactivate();
                        }
                        teleportable.PlaceAt(GlobalPosition);
                        EmitSignal(SignalName.OnTeleportEnd, node);
                    }
                }
            }
		}

        public void Teleport(string location){
            foreach(var node in Global.Instance.PlayerSystem.GetChildren()){
                if(node is ITeleportable teleportable){
                    teleportable.TeleportToID = ID;
                    EmitSignal(SignalName.OnTeleportStart);
                    Global.Instance.LocationLoader.LoadLocation(location);
                }
            }
        }
        public void PrepareTeleportation(string location){
            Global.Instance.LocationLoader.StartPackingScene();
        }
    }
}