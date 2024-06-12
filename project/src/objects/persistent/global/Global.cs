using Godot;
using System;
using System.Linq;
using UI;

namespace Game {
	/// <summary>
	/// Глобальный класс. Его следует использовать аккуратно.
	/// Нужен для регистрации только тех объектов и экземпляров классов, которые однозначно не могут создаваться в игре несколько раз.
	/// И которые логично хранить в глобальном пространстве.
	/// </summary>
	public partial class Global : Node3D
	{
		[Export]
		PackedScene UIScene;
		UIDad UIInstance;
		[Export]
		public AudioPlayer audioPlayer;
		[Export]
		public Node3D PlayerSystem;
		
		/// <summary>
		/// использовать осторожно.
		/// желательно только в UI
		/// </summary>
		public HandDude CurrentPlayer;

		[Export]
		public LocationLoader LocationLoader;

		private MainCamera _mainCamera;
		public MainCamera CurrentMainCamera { get { return _mainCamera; } }

		private GameSettings settings = new GameSettings();
		public GameSettings Settings { get { return settings; } }

        #region singleton
        private static Global _instance = null;
		public static Global Instance {	get { return _instance; } }

		public bool DebugVisibility = false;
		public event Action<bool> OnDebugVisibilityChange;

		public Resources resources = new Resources();

		public Node3D GameScene;

		public Global()
		{
			_instance = this;
		}
        #endregion

        public void SetCurrentMainCamera(MainCamera camera)
        {
			_mainCamera = camera;
        }

		public override void _Ready()
		{
			// Вызываем DeferredReady отложенно, т.е. в следующем такте игры,
			// после _Ready, когда все что на сцене уже точно на 100% прогружено.
			CallDeferred("DeferredReady");
		}
		public void LoadGameScene(PackedScene gameScene){
			GameScene.QueueFree();
			GameScene = gameScene.Instantiate<Node3D>();
			UIInstance.SceneContainer.AddChild(GameScene);
		}
		
		/// <summary>
		/// берем сцену игры и отдаем UI чтобы он поместил ее в нужный viewport
		/// </summary>
		void SetupUI(){
			if(UIScene==null) return;
			var ui = UIScene.Instantiate<UIDad>();
			UIInstance = ui;
			AddChild(ui);
			var neighbours = GetParent().GetChildren();
			// типо берем то что кроме Global, т.к. в /root/ спавнятся только Global и запускаемая сцена
			var scene = neighbours.Last();
			GameScene = (Node3D)scene;
			
			ui.PlaceNode(scene);
			ui.PlaceNode(PlayerSystem);
		}
		void SetupAudioPlayer(){
			audioPlayer.Reparent(GameScene);
		}
		public void DeferredReady()
        {
			SetupUI();
			SetupAudioPlayer();

			foreach(var node in PlayerSystem.GetChildren()){
                if(node is HandDude player){
                    CurrentPlayer = player;
					break;
                }
            }

			Settings.InvokeChange();
		}

		public override void _Process(double delta)
		{
			if(Input.IsActionJustPressed("debug_visibility")){
				DebugVisibility = !DebugVisibility;
				OnDebugVisibilityChange?.Invoke(DebugVisibility);
			}
		}
	}
}
