using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Описывает объект за которым можно наблюдать
    /// </summary>
    public interface IViewable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Vector3 - точка наблюдения</returns>
        public Vector3 GetViewTargetPoint();
    }

    /// <summary>
    /// Описывает объект который может следить за типом T.
    /// </summary>
    /// <typeparam name="T">Подразумевается IViewable, но можно установить что угодно</typeparam>
    public interface IViewFollower<T>
    {
        /// <summary>
        /// Текущий объект за которым наблюдаем. Публичное поле для чтения, но не записи
        /// </summary>
        public T ViewTarget { get; }
        /// <summary>
        /// Устанавливает Текущий объект наблюдения.
        /// </summary>
        /// <param name="target">объект наблюдения</param>
        public void SetViewTarget(T target);
    }
}