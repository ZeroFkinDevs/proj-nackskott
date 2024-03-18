using Godot;
using System;

namespace Game
{
    public partial class InventoryItem : Resource
    {
        [Export]
        public string Name;
        [Export]
        public string Type;

        [Export]
        public Godot.Collections.Dictionary<string, Godot.GodotObject> Data;
    }
}