using System.IO;
using System.Threading.Tasks;
using Godot;

namespace Game {
	public partial class LocationLoader : Resource
	{
        [Export]
        public Godot.Collections.Dictionary<string, PackedScene> locations;

        public string CurrentLocationId = null;
        protected TaskCompletionSource<string> SavingTask;

        public async void LoadLocation(string id){
            PackedScene scene = null;
            locations.TryGetValue(id, out scene);
            if(scene != null){
                var packed = PackIntoScene(Global.Instance.GameScene);
                locations[CurrentLocationId] = packed;
                CurrentLocationId = id;
                
                Global.Instance.LoadGameScene(scene);
            }
        }

        public void StartPackingScene(){
            Task.Run(() =>
            {
                var packed = PackIntoScene(Global.Instance.GameScene);
                locations[CurrentLocationId] = packed;
                SavingTask.SetResult(CurrentLocationId);
            });
        }

        public PackedScene PackIntoScene(Node3D sceneNode){
            foreach(var node in sceneNode.FindChildren("*", recursive: true, owned: false)){
                node.Owner = sceneNode;
            }
            var scenePack = new PackedScene();
            Error result = scenePack.Pack(sceneNode);
            if (result == Error.Ok)
            {
                SavingTask = new TaskCompletionSource<string>();
                Task.Run(() =>
                {
                    var dir = "user://test-save";
                    string globalDirPath = ProjectSettings.GlobalizePath(dir);
                    Directory.CreateDirectory(globalDirPath);
                    var path = dir+"/"+CurrentLocationId+".tscn";

                    Error error = ResourceSaver.Save(scenePack, path, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
                    if (error != Error.Ok)
                    {
                        GD.PushError("An error occurred while saving the scene to disk.");
                        SavingTask.SetResult(null);
                    }else{
                        SavingTask.SetResult(path);
                    }
                });
                return scenePack;
            }
            return scenePack;
        }

        public string GuessCurrentLocId(Node3D scene){
            foreach (var locId in locations.Keys)
            {
                PackedScene loc = null;
                locations.TryGetValue(locId, out loc);
                if (loc.ResourcePath == scene.SceneFilePath){
                    CurrentLocationId = locId;
                    break;
                }
            }
            return CurrentLocationId;
        }
    }
}