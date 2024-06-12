using Godot;
using System;

namespace Game
{
    public partial class Teleporter : Node3D
    {
        [Export]
        public string ID;

        [Signal]
		public delegate void OnTeleportStartEventHandler();
        [Signal]
		public delegate void OnTeleportEndEventHandler(HandDude player);

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
            foreach(var node in Global.Instance.PlayerSystem.GetChildren()){
                if(node is HandDude player){
                    if(player.TeleportToID == ID){
                        player.PlaceAt(GlobalPosition);
                        EmitSignal(SignalName.OnTeleportEnd, player);
                    }
                }
            }
		}

        public void Teleport(string location){
            foreach(var node in Global.Instance.PlayerSystem.GetChildren()){
                if(node is HandDude player){
                    player.TeleportToID = ID;
                    EmitSignal(SignalName.OnTeleportStart);
                    Global.Instance.LocationLoader.LoadLocation(location);
                }
            }
        }
    }
}