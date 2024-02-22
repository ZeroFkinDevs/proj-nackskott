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
	public partial class Global : Node
	{
		[Export]
		PackedScene UIScene;

		private MainCamera _mainCamera;
		public MainCamera CurrentMainCamera { get { return _mainCamera; } }

		private GameSettings settings = new GameSettings();
		public GameSettings Settings { get { return settings; } }

        #region singleton
        private static Global _instance = null;
		public static Global Instance {	get { return _instance; } }
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
		
		/// <summary>
		/// берем сцену игры и отдаем UI чтобы он поместил ее в нужный viewport
		/// </summary>
		void SetupUI(){
			var ui = UIScene.Instantiate<UIDad>();
			AddChild(ui);
			var neighbours = GetParent().GetChildren();
			// типо берем то что кроме Global, т.к. в /root/ спавнятся только Global и запускаемая сцена
			var scene = neighbours.Last();
			ui.PlaceGameScene(scene);
		}
		public void DeferredReady()
        {
			SetupUI();
			Settings.InvokeChange();
		}

		public override void _Process(double delta)
		{

		}
	}
}
