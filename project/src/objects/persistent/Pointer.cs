using Godot;
using System;

namespace Game
{
	/// <summary>
	/// Точка в 3d пространстве которая следует 
	/// указателю мыши по плоскости в пространстве.  <br/>
	/// 
	/// Она не зависит напрямую от Мыши, поэтому <br/>
	/// поэтому можно будет добавлять ей либо физические действия, 
	/// по типу столкновений с миром
	/// </summary>
	public partial class Pointer : Node3D
	{
		// у Pointer есть рабочая плоскость, по которой он передвигается,
		// но можно будет сделать чтобы он передвигался по миру как character, если что.
		// в этом и прикол иметь отдельный нод для указателя.
		/// <summary>
		/// Виртуальный плейн, нужен только чтобы отбросить на него луч из камеры 
		/// и переместить Pointer в точку пересечения. <br/>
		/// Инициализирован вертикальной плоскостью Up и позицией в нулевом векторе (Zero) в мире.
		/// </summary>
		public Plane workingPlane = new Plane(Vector3.Up, Vector3.Zero);

		/// <summary>
		/// Проецирует точку на экране под курсором на workingPlane в мировом пространстве игры <br/>
		/// Перемещает Pointer
		/// </summary>
		/// <param name="delta">на всяки случай может понадобиться плавное перемещение</param>
		public void MoveToMouseCursor(double delta)
        {
			// подготавливаем переменные
			Camera3D camera = Global.Instance.CurrentMainCamera.Camera;
			if(!camera.Current) return;
			float rayCastDistance = camera.Far;

			// получаем начальную и конечную точки луча отбрасываемого из камеры в направлении курсора мыши в пространстве экрана
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 rayFrom = camera.ProjectRayOrigin(mousePos);
			Vector3 rayTo = camera.ProjectRayNormal(mousePos) * camera.Far;

			// считаем точку пересечения
			Vector3? worldPos = workingPlane.IntersectsRay(rayFrom, rayTo);
			// проверяем тип? Nullable на то есть ли у него значение
			if (worldPos != null)
			{
				// если есть получаем НЕнулевое значение
				// P.S. Value это относиться к Nullable<T>, а не к вектору
				Vector3 pos = worldPos.Value;
				// перемещаем Pointer
				GlobalPosition = new Vector3(pos.X, workingPlane.Y, pos.Z);
			}
		}

        public override void _Process(double delta)
        {
			MoveToMouseCursor(delta);
		}
    }
}