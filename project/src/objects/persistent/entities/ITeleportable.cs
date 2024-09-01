using Godot;
using System;

namespace Game
{
	public interface ITeleportable
	{
        public void PlaceAt(Vector3 position);
        public string TeleportToID{get;set;}
		public TeleportableObjectType GetObjectType();
		public void Deactivate();
		public void Activate();
	}
	public enum TeleportableObjectType{
		DUDE,
		HAND_DUDE,
	}
}