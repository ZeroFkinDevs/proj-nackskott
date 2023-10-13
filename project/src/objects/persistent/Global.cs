using Godot;
using System;

namespace Game {
	public partial class Global : Node
	{
		private MainCamera _mainCamera;
		public MainCamera CurrentMainCamera { get { return _mainCamera; } }

		private static Global _instance = null;
		public static Global Instance {	get { return _instance; } }

		public void SetCurrentMainCamera(MainCamera camera)
        {
			_mainCamera = camera;
        }

		public Global()
        {
			_instance = this;
        }

		public override void _Ready()
		{

		}

		public override void _Process(double delta)
		{

		}
	}
}
