using Godot;

namespace Game {
	public partial class LocationLoader : Resource
	{
        [Export]
        public Godot.Collections.Dictionary<string, PackedScene> locations;

        public void LoadLocation(string id){
            PackedScene scene = null;
            locations.TryGetValue(id, out scene);
            if(scene != null){
                Global.Instance.LoadGameScene(scene);
            }
        }
    }
}