using Godot;
using System;

namespace Game
{
	/// <summary>
	/// Класс позволяющий плавно перемещать какой-нибудь Node3D. <br/>
	/// Также релизует привязку к сетке. <br/>
	/// Можно переместить в отдельный файл - С камерой он напрямую никак не связан, он о ней не знает.
	/// </summary>
	public class SmoothTranslater
    {
		/// <summary>
		/// Целевая точка куда надо переместиться. доступна и для чтения и для записи.
		/// </summary>
		public Vector3 TargetPoint;
		/// <summary>
		/// Позиция которя сглаживается.
		/// </summary>
		/// asd
		public Vector3 ActualPoint;

		private Node3D node;
		public float MotionSmoothness = 2.0f;
		public float TargetSmoothness = 2.0f;

		// Зернистость сетки
		public float Snapping = 40.0f;

		public SmoothTranslater(Node3D _node, float motionSmoothness = 2.0f)
		{
			node = _node;
			TargetPoint = node.GlobalPosition;
			ActualPoint = TargetPoint;
			MotionSmoothness = motionSmoothness;
			TargetSmoothness = motionSmoothness;
		}
		/// <summary>
		/// Плавного перемещает в точку TargetPoint
		/// </summary>
		/// <param name="delta"></param>
		public void UpdateMotion(double delta)
		{
			// ActualPoint без привязки к сетке, поэтому 
			MotionSmoothness += (TargetSmoothness - MotionSmoothness) * ((float)delta * 1.5f);
			ActualPoint = ActualPoint.Lerp(TargetPoint, (float)delta * MotionSmoothness);
			var finalPos = ActualPoint;
			node.GlobalPosition = ActualPoint;
			
			// snapping
			Vector3 snappedPos = (ActualPoint * Snapping).Floor() / Snapping;
			finalPos = snappedPos;
			if ((finalPos - TargetPoint).Length() <= (1.0f/Snapping)*100.0f){
			}

			// Обновляем позицию объекта.
			node.GlobalPosition = finalPos;
		}
	}

	/// <summary>
	/// Класс камеры который реализует IViewFollower с T = Node3D <br/>
	/// Плавно перемещается в точку объекта слежения
	/// </summary>
	public partial class MainCamera : Node3D, IViewFollower<Node3D>
	{
		[Export]
		public Camera3D Camera;
		[Export]
		public Camera3D FilmCamera;

		[Export]
		public AudioListener3D AudioListener;

		/// <summary>
		/// Путь к объекту слежения
		/// </summary>
		[Export]
		public NodePath ViewableTargetPath;
		/// <summary>
		/// Плавность перемещения
		/// </summary>
		[Export]
		public float MotionSmoothness = 2.0f;

		private SmoothTranslater smoothTranslater;
		public SmoothTranslater SmoothTrans {get{return smoothTranslater;}}

		public Node3D AttachNode = null;
		public Transform3D NormalTransform;

		public MainCamera()
		{
			// регистрируем и устанавливаем камеру как текущую в Global
			Global.Instance.SetCurrentMainCamera(this);
			// подписываемся на изменение настроек
			Global.Instance.Settings.OnSettingsChanged += ApplySettings;
		}
		public override void _Ready()
		{
			SetViewTarget(GetNode<Node3D>(ViewableTargetPath));

			smoothTranslater = new SmoothTranslater(this, MotionSmoothness);

			NormalTransform = Camera.Transform;
		}
		public void UpdateSnapping()
		{
			smoothTranslater.Snapping = GetViewport().GetVisibleRect().Size.Y / Camera.Size;
		}
        public override void _Process(double delta)
        {
			if(AttachNode!=null){
				FilmCamera.GlobalTransform = AttachNode.GlobalTransform;
			}
			else{
				FollowViewTarget(delta);
				smoothTranslater.UpdateMotion(delta);
				UpdateSnapping();
			}
        }

		#region Target and IViewFollower

		/// <summary>
		/// Присваивается из SetViewTarget
		/// </summary>
		private Node3D currentViewTarget = null;
		public Node3D ViewTarget { get { return currentViewTarget; } }
		/// <summary>
		/// Целевая точка куда нужно переместиться
		/// </summary>
		private Vector3 currentTargetPoint;

		public void SetViewTarget(Node3D target)
		{
			currentViewTarget = target;
		}
		/// <summary>
		/// Получает точку от объекта слежения и обновляет свою точку куда нужно переместиться
		/// </summary>
		/// <param name="delta"></param>
		private void FollowViewTarget(double delta)
        {
			Vector3 point = GlobalPosition;
			// Проверяем тип ViewTarget и получаем точки слежения
			// если ViewTarget это IViewable, то очень удобно преобразовываем Node3D в IViewable сохраняя в переменную viewable
			if (ViewTarget is IViewable viewable)
            {
				point = viewable.GetViewTargetPoint();
            }
			// если нет то просто позицию Node3D, вдруг ViewTarget не реализует IViewable, это можно допустить.
			else
			{
				point = ViewTarget.GlobalPosition;
            }
			smoothTranslater.TargetPoint = point;
		}
		#endregion

		private void ApplySettings(GameSettings settings)
        {
			// меняем что-то зависимо от установленных настроек игры
			// например если стоит DEBUG режим, включаем перспективное отображение. наоборот - нормальное, ортографическое
			if (settings.IsInDebug) Camera.SetPerspective(Camera.Fov, Camera.Near, Camera.Far);
			else Camera.SetOrthogonal(Camera.Size, Camera.Near, Camera.Far);
		}

		public void StartAttachedPerspectiveSession(Node3D attachTo){
			AttachNode = attachTo;
			FilmCamera.Current = true;
			Camera.Current = false;
			AudioListener?.ClearCurrent();
		}
		public void DeAttach(){
			AttachNode = null;
			FilmCamera.Current = false;
			Camera.Current = true;
			smoothTranslater.ActualPoint = new Vector3(FilmCamera.GlobalPosition.X, Camera.GlobalPosition.Y, FilmCamera.GlobalPosition.Z);
			smoothTranslater.MotionSmoothness = 0.0f;
			AudioListener?.MakeCurrent();
		}
    }
}
