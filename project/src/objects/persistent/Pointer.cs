using Godot;
using System;

namespace Game
{
	public partial class Pointer : Node3D
	{
		private Plane workingPlane = new Plane(Vector3.Up, Vector3.Zero);

		public Pointer()
		{
			
		}
        public override void _Process(double delta)
        {
			Vector2 mousePos = GetViewport().GetMousePosition();
			Camera3D camera = Global.Instance.CurrentMainCamera.Camera;
			float rayCastDistance = camera.Far;

			Vector3 rayFrom = camera.ProjectRayOrigin(mousePos);
			Vector3 rayTo = camera.ProjectRayNormal(mousePos) * camera.Far;
			Vector3? worldPos = workingPlane.IntersectsRay(rayFrom, rayTo);
            if (worldPos != null)
            {
				Vector3 pos = worldPos.Value;
				Position = new Vector3(pos.X, Position.Y, pos.Z);
			}
		}
    }
}
