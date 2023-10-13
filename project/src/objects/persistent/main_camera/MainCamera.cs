using Godot;
using System;

namespace Game
{
	public partial class MainCamera : Node3D, IViewFollower<Node3D>
	{
		[Export]
		public NodePath CameraPath;
		private Camera3D _camera = null;
		public Camera3D Camera { get { return _camera; } }

		[Export]
		public NodePath ViewableTargetPath;
		[Export]
		public float MotionHardness = 2.0f;

		public MainCamera()
		{
			Global.Instance.SetCurrentMainCamera(this);
		}
		public override void _Ready()
		{
			_camera = GetNode<Camera3D>(CameraPath);
			SetViewTarget(GetNode<Node3D>(ViewableTargetPath));
		}
        public override void _Process(double delta)
        {
			FollowViewTarget(delta);
			UpdateMotion(delta);
        }

        // Target and IViewFollower
        private Node3D currentViewTarget = null;
		public Node3D ViewTarget { get { return currentViewTarget; } }
		private Vector3 currentTargetPoint;

		public void SetViewTarget(Node3D target)
		{
			currentViewTarget = target;
		}
		private void FollowViewTarget(double delta)
        {
			Vector3 point = Position;
			if(ViewTarget is IViewable viewable)
            {
				point = viewable.GetViewTargetPoint();
            }
            else
            {
				point = ViewTarget.Position;
            }
			currentTargetPoint = point;
		}
		private void UpdateMotion(double delta)
        {
			Position = Position.Lerp(currentTargetPoint, (float)delta * MotionHardness);
        }
		// End -- Target and IViewFollower
	}
}
