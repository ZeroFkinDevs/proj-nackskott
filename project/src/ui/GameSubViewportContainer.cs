using Godot;
using System;

namespace Game
{
	public partial class GameSubViewportContainer : SubViewportContainer
	{
		private void UpdateSize()
		{
			Size = GetParent<Control>().Size + new Vector2(4, 4);
		}
		public void UpdateOffset()
		{
			var mainCamera = Global.Instance.CurrentMainCamera;
			var pixDist = mainCamera.SmoothTrans.ActualPoint - mainCamera.Position;
			float pixPerMeter = Size.Y / mainCamera.Camera.Size;
			var dist2d = new Vector2(pixDist.X, pixDist.Z) * (pixPerMeter);
			dist2d *= -1;
			Position = dist2d;
			GD.Print(dist2d);
		}
        public override void _Process(double delta)
        {
			UpdateSize();
			CallDeferred("UpdateOffset");
        }
    }
}